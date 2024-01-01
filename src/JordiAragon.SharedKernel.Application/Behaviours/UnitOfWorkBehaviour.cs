namespace JordiAragon.SharedKernel.Application.Behaviours
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Exceptions;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class UnitOfWorkBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse> // TODO: Review restrict to ICommand<TResponse> or ICommand
        where TResponse : IResult
    {
        private readonly ILogger<TRequest> logger;
        private readonly ICurrentUserService currentUserService;
        private readonly IUnitOfWork unitOfWork;

        public UnitOfWorkBehaviour(
            ILogger<TRequest> logger,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                this.unitOfWork.BeginTransaction();

                var response = await next();

                // Get Ardalis.Result.IsSuccess or Ardalis.Result<T>.IsSuccess
                var isSuccessResponse = typeof(TResponse).GetProperty("IsSuccess").GetValue(response, null);
                if ((bool)isSuccessResponse)
                {
                    await this.unitOfWork.CommitTransactionAsync();
                }
                else
                {
                    this.unitOfWork.RollbackTransaction();
                }

                return response;
            }
            catch (NotFoundException notFoundException)
            {
                this.unitOfWork.RollbackTransaction();

                var resultType = typeof(Result);
                if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
                {
                    resultType = typeof(Result<>).MakeGenericType(typeof(TResponse).GetGenericArguments()[0]);
                }

                // Get Ardalis.Result.NotFound or Ardalis.Result<T>.NotFound method.
                var notFoundMethod = resultType.GetMethod("NotFound", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string[]) }, null);
                var result = notFoundMethod.Invoke(resultType, new[] { new[] { notFoundException.Message } });

                return (TResponse)result;
            }
            catch (BusinessRuleValidationException businessRuleValidationException)
            {
                this.unitOfWork.RollbackTransaction();

                var errors = new List<ValidationError>()
                {
                    new ValidationError()
                    {
                        ErrorMessage = businessRuleValidationException.Message,
                        Identifier = businessRuleValidationException.BrokenRule.GetType().Name,
                        Severity = ValidationSeverity.Error,
                    },
                };

                // Get Ardalis.Result.Invalid or Ardalis.Result<T>.Invalid method.
                var resultInvalidMethod = typeof(TResponse).GetMethod("Invalid", BindingFlags.Static | BindingFlags.Public);

                return (TResponse)resultInvalidMethod.Invoke(null, new object[] { errors });
            }
            catch (Exception exception)
            {
                this.unitOfWork.RollbackTransaction();

                var requestName = typeof(TRequest).Name;
                var userId = this.currentUserService.UserId ?? string.Empty;
                var requestSerialized = JsonSerializer.Serialize(request);

                this.logger.LogError(
                    exception,
                    "Unhandled Exception. Request: {RequestName}  User ID: {@UserId} Request Data: {RequestSerialized}",
                    requestName,
                    userId,
                    requestSerialized);

                throw;
            }
        }
    }
}
