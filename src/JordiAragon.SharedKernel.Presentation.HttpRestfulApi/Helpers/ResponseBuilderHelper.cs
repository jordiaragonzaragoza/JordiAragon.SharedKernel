namespace JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using FluentValidation.Results;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public static class ResponseBuilderHelper
    {
        public static object BuildResponse(List<ValidationFailure> failures, HttpContext context, int statusCode)
        {
            switch (statusCode)
            {
                case (int)HttpStatusCode.UnprocessableEntity:
                    return UnprocessableEntity(failures, context, statusCode);

                case (int)HttpStatusCode.NotFound:
                    return NotFound(failures, context, statusCode);

                case (int)HttpStatusCode.Conflict:
                    return Conflict(failures, context, statusCode);

                default:
                    return Invalid(failures, context, statusCode);
            }
        }

        private static object Invalid(List<ValidationFailure> failures, HttpContext context, int statusCode)
        {
            return new ValidationProblemDetails(
                failures.GroupBy(f => f.PropertyName)
                    .ToDictionary(
                        keySelector: e => e.Key,
                        elementSelector: e => e.Select(m => m.ErrorMessage).ToArray()))
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "One or more validation errors occurred.",
                Status = statusCode,
                Instance = context.Request.Path,
                Extensions = { { "traceId", context.TraceIdentifier } },
            };
        }

        private static object Conflict(List<ValidationFailure> failures, HttpContext context, int statusCode)
        {
            return new ProblemDetails
            {
                Type = "https://www.rfc-editor.org/rfc/rfc9110#name-409-conflict",
                Title = "There was a conflict.",
                Status = statusCode,
                Instance = context.Request.Path,
                Extensions = { { "traceId", context.TraceIdentifier } },
                Detail = failures.Any() ? PrepareDetails(failures) : null,
            };
        }

        private static object UnprocessableEntity(List<ValidationFailure> failures, HttpContext context, int statusCode)
        {
            return new ProblemDetails
            {
                Type = "https://www.rfc-editor.org/rfc/rfc9110#name-422-unprocessable-content",
                Title = "Something went wrong.",
                Status = statusCode,
                Instance = context.Request.Path,
                Extensions = { { "traceId", context.TraceIdentifier } },
                Detail = failures.Any() ? PrepareDetails(failures) : null,
            };
        }

        private static object NotFound(List<ValidationFailure> failures, HttpContext context, int statusCode)
        {
            return new ProblemDetails()
            {
                Title = "Resource not found.",
                Type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.4",
                Status = statusCode,
                Instance = context.Request.Path,
                Extensions = { { "traceId", context.TraceIdentifier } },
                Detail = failures.Any() ? PrepareDetails(failures) : null,
            };
        }

        private static string PrepareDetails(List<ValidationFailure> failures)
        {
            var details = new StringBuilder("Next error(s) occured:");

            foreach (var error in failures)
            {
                details.Append("* ").Append(error).AppendLine();
            }

            return details.ToString();
        }
    }
}