namespace JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using FastEndpoints;
    using FluentValidation;
    using FluentValidation.Results;

    public static class EndpointExtensions
    {
        public static async Task SendResponseAsync(this IEndpoint endpoint, IResult result, CancellationToken cancellationToken)
        {
            switch (result.Status)
            {
                case ResultStatus.Ok:
                    if (result is Result)
                    {
                        await endpoint.HttpContext.Response.SendNoContentAsync(cancellationToken);
                        break;
                    }

                    await endpoint.HttpContext.Response.SendAsync(result.GetValue(), cancellation: cancellationToken);
                    break;

                case ResultStatus.Error:
                    PrepareValidationFailures(endpoint, result.Errors);
                    await endpoint.HttpContext.Response.SendErrorsAsync(endpoint.ValidationFailures, statusCode: (int)HttpStatusCode.UnprocessableEntity, cancellation: cancellationToken);
                    break;

                case ResultStatus.Forbidden:
                    await endpoint.HttpContext.Response.SendForbiddenAsync(cancellationToken);
                    break;

                case ResultStatus.Unauthorized:
                    await endpoint.HttpContext.Response.SendUnauthorizedAsync(cancellationToken);
                    break;

                case ResultStatus.Invalid:
                    PrepareValidationFailures(endpoint, result.ValidationErrors);
                    await endpoint.HttpContext.Response.SendErrorsAsync(endpoint.ValidationFailures, statusCode: (int)HttpStatusCode.BadRequest, cancellation: cancellationToken);
                    break;

                case ResultStatus.NotFound:
                    PrepareValidationFailures(endpoint, result.Errors);
                    await endpoint.HttpContext.Response.SendErrorsAsync(endpoint.ValidationFailures, statusCode: (int)HttpStatusCode.NotFound, cancellation: cancellationToken);
                    break;

                case ResultStatus.Conflict:
                    PrepareValidationFailures(endpoint, result.Errors);
                    await endpoint.HttpContext.Response.SendErrorsAsync(endpoint.ValidationFailures, statusCode: (int)HttpStatusCode.Conflict, cancellation: cancellationToken);
                    break;
            }
        }

        private static void PrepareValidationFailures(IEndpoint endpoint, IEnumerable<ValidationError> validationErrors)
        {
            validationErrors.ToList().ForEach(e =>
                                    endpoint.ValidationFailures.Add(new ValidationFailure(e.Identifier, e.ErrorMessage) { ErrorCode = e.ErrorCode, Severity = FromSeverity(e.Severity) }));
        }

        private static void PrepareValidationFailures(IEndpoint endpoint, IEnumerable<string> errors)
        {
            foreach (var error in errors)
            {
                endpoint.ValidationFailures.Add(new ValidationFailure() { ErrorMessage = error });
            }
        }

        private static Severity FromSeverity(ValidationSeverity severity)
        {
            return severity switch
            {
                ValidationSeverity.Error => Severity.Error,
                ValidationSeverity.Warning => Severity.Warning,
                ValidationSeverity.Info => Severity.Info,
                _ => throw new ArgumentOutOfRangeException(nameof(severity), "Unexpected Severity"),
            };
        }
    }
}