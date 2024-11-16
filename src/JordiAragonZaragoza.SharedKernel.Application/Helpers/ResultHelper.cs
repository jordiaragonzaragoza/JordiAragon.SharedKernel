namespace JordiAragonZaragoza.SharedKernel.Application.Helpers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Ardalis.Result;

    public static class ResultHelper
    {
        public static Result<TDestination> HandleNonSuccessStatus<TSource, TDestination>(this Result<TSource> result)
        {
            ArgumentNullException.ThrowIfNull(result, nameof(result));

            return result.Status switch
            {
                ResultStatus.NotFound => result.Errors.Any()
                    ? Result<TDestination>.NotFound(result.Errors.ToArray())
                    : Result<TDestination>.NotFound(),
                ResultStatus.Unauthorized => result.Errors.Any()
                    ? Result<TDestination>.Unauthorized(result.Errors.ToArray())
                    : Result<TDestination>.Unauthorized(),
                ResultStatus.Forbidden => result.Errors.Any()
                    ? Result<TDestination>.Forbidden(result.Errors.ToArray())
                    : Result<TDestination>.Forbidden(),
                ResultStatus.Invalid => Result<TDestination>.Invalid(result.ValidationErrors),
                ResultStatus.Error => Result<TDestination>.Error(new ErrorList(result.Errors.ToArray(), result.CorrelationId)),
                ResultStatus.Conflict => result.Errors.Any()
                    ? Result<TDestination>.Conflict(result.Errors.ToArray())
                    : Result<TDestination>.Conflict(),
                ResultStatus.CriticalError => Result<TDestination>.CriticalError(result.Errors.ToArray()),
                ResultStatus.Unavailable => Result<TDestination>.Unavailable(result.Errors.ToArray()),
                ResultStatus.NoContent => Result<TDestination>.NoContent(),
                _ => throw new NotSupportedException($"Result {result.Status} conversion is not supported."),
            };
        }

        public static Result<TDestination> HandleNonSuccessStatus<TDestination>(this Result result)
        {
            ArgumentNullException.ThrowIfNull(result, nameof(result));

            return result.Status switch
            {
                ResultStatus.NotFound => result.Errors.Any()
                    ? Result<TDestination>.NotFound(result.Errors.ToArray())
                    : Result<TDestination>.NotFound(),
                ResultStatus.Unauthorized => result.Errors.Any()
                    ? Result<TDestination>.Unauthorized(result.Errors.ToArray())
                    : Result<TDestination>.Unauthorized(),
                ResultStatus.Forbidden => result.Errors.Any()
                    ? Result<TDestination>.Forbidden(result.Errors.ToArray())
                    : Result<TDestination>.Forbidden(),
                ResultStatus.Invalid => Result<TDestination>.Invalid(result.ValidationErrors),
                ResultStatus.Error => Result<TDestination>.Error(new ErrorList(result.Errors.ToArray(), result.CorrelationId)),
                ResultStatus.Conflict => result.Errors.Any()
                    ? Result<TDestination>.Conflict(result.Errors.ToArray())
                    : Result<TDestination>.Conflict(),
                ResultStatus.CriticalError => Result<TDestination>.CriticalError(result.Errors.ToArray()),
                ResultStatus.Unavailable => Result<TDestination>.Unavailable(result.Errors.ToArray()),
                ResultStatus.NoContent => Result<TDestination>.NoContent(),
                _ => throw new NotSupportedException($"Result {result.Status} conversion is not supported."),
            };
        }

        public static Result HandleNonSuccessStatus<TSource>(this Result<TSource> result)
        {
            ArgumentNullException.ThrowIfNull(result, nameof(result));

            return result.Status switch
            {
                ResultStatus.NotFound => result.Errors.Any()
                    ? Result.NotFound(result.Errors.ToArray())
                    : Result.NotFound(),
                ResultStatus.Unauthorized => result.Errors.Any()
                    ? Result.Unauthorized(result.Errors.ToArray())
                    : Result.Unauthorized(),
                ResultStatus.Forbidden => result.Errors.Any()
                    ? Result.Forbidden(result.Errors.ToArray())
                    : Result.Forbidden(),
                ResultStatus.Invalid => Result.Invalid(result.ValidationErrors),
                ResultStatus.Error => Result.Error(new ErrorList(result.Errors.ToArray(), result.CorrelationId)),
                ResultStatus.Conflict => result.Errors.Any()
                    ? Result.Conflict(result.Errors.ToArray())
                    : Result.Conflict(),
                ResultStatus.CriticalError => Result.CriticalError(result.Errors.ToArray()),
                ResultStatus.Unavailable => Result.Unavailable(result.Errors.ToArray()),
                ResultStatus.NoContent => Result.NoContent(),
                _ => throw new NotSupportedException($"Result {result.Status} conversion is not supported."),
            };
        }

        public static string ResultDetails(this IResult result)
        {
            ArgumentNullException.ThrowIfNull(result, nameof(result));

            switch (result.Status)
            {
                case ResultStatus.Ok: return Success(result);
                case ResultStatus.NotFound: return NotFoundEntity(result);
                case ResultStatus.Unauthorized: return "Unauthorized.";
                case ResultStatus.Forbidden: return "Forbidden.";
                case ResultStatus.Invalid: return BadRequest(result);
                case ResultStatus.Error: return UnprocessableEntity(result);
                default:
                    throw new NotSupportedException($"Result {result.Status} conversion is not supported.");
            }
        }

        private static string Success(IResult result)
        {
            var details = new StringBuilder("Success. ");

            if (result is Result)
            {
                return details.ToString();
            }

            details.Append(result.GetValue());

            return details.ToString();
        }

        private static string NotFoundEntity(IResult result)
        {
            var details = new StringBuilder("Resource not found. ");

            if (result.Errors.Any())
            {
                details.Append("Next error(s) occured: ");

                foreach (var error in result.Errors)
                {
                    details.Append(error);
                }
            }

            return details.ToString();
        }

        private static string BadRequest(IResult result)
        {
            var details = new StringBuilder("Bad Request. ");

            var errors = result.ValidationErrors
                .GroupBy(x => x.Identifier, x => x.ErrorMessage)
                .ToDictionary(g => g.Key, g => g.ToArray());

            if (errors.Count > 0)
            {
                details.Append("Next validation error(s) occured: ");

                foreach (var error in errors)
                {
                    details.Append(CultureInfo.InvariantCulture, $"Identifier: {error.Key}. Message: {error.Value}");
                }
            }

            return details.ToString();
        }

        private static string UnprocessableEntity(IResult result)
        {
            var details = new StringBuilder("Error. Something went wrong. ");

            if (result.Errors.Any())
            {
                details.Append("Next error(s) occured: ");

                foreach (var error in result.Errors)
                {
                    details.Append(error);
                }
            }

            return details.ToString();
        }
    }
}