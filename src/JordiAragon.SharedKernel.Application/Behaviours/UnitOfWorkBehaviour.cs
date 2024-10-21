﻿namespace JordiAragon.SharedKernel.Application.Behaviours
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Exceptions;
    using MediatR;

    using NotFoundException = JordiAragon.SharedKernel.Domain.Exceptions.NotFoundException;

    public class UnitOfWorkBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, IBaseCommand
        where TResponse : IResult
    {
        private readonly IUnitOfWork unitOfWork;

        public UnitOfWorkBehaviour(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            Guard.Against.Null(next, nameof(next));

            try
            {
                return await this.unitOfWork.ExecuteInTransactionAsync(async () =>
                {
                    return await next();
                });
            }

            // TODO: Remove catches when not using custom exceptions on domain layer.
            catch (NotFoundException notFoundException)
            {
                var resultType = typeof(Result);
                if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
                {
                    resultType = typeof(Result<>).MakeGenericType(typeof(TResponse).GetGenericArguments()[0]);
                }

                // Get Ardalis.Result.NotFound or Ardalis.Result<T>.NotFound method.
                var notFoundMethod = resultType.GetMethod("NotFound", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string[]) }, null)
                    ?? throw new InvalidOperationException("The 'NotFound' method was not found on type " + typeof(TResponse).FullName);

                var result = notFoundMethod.Invoke(resultType, new[] { new[] { notFoundException.Message } })
                    ?? throw new InvalidOperationException("The 'NotFound' method returned null.");

                return (TResponse)result;
            }

            // TODO: Remove catches when not using custom exceptions on domain layer.
            catch (BusinessRuleValidationException businessRuleValidationException)
            {
                var errors = new List<ValidationError>()
                {
                    new ValidationError()
                    {
                        ErrorMessage = businessRuleValidationException.Message,
                        Identifier = businessRuleValidationException.BrokenRule.GetType().Name,
                        Severity = ValidationSeverity.Error,
                    },
                };

                // Get Ardalis.Result.Invalid(List<ValidationError> validationErrors) or Ardalis.Result<T>.Invalid(List<ValidationError> validationErrors) method.
                var resultInvalidMethod = typeof(TResponse).GetMethod("Invalid", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(List<ValidationError>) }, null)
                    ?? throw new InvalidOperationException("The 'Invalid' method was not found on type " + typeof(TResponse).FullName);

                var result = resultInvalidMethod.Invoke(null, new object[] { errors })
                    ?? throw new InvalidOperationException("The 'Invalid' method returned null.");

                return (TResponse)result;
            }
        }
    }
}