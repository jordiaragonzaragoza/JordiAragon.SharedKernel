namespace JordiAragon.SharedKernel.Application.Helpers
{
    using System;
    using System.Linq;
    using System.Text;
    using Ardalis.Result;

    public static class ResultHelper
    {
        public static Result<TDestination> Transform<TSource, TDestination>(this Result<TSource> result, Func<TSource, TDestination> func)
        {
            switch (result.Status)
            {
                case ResultStatus.Ok: return func(result);
                case ResultStatus.NotFound: return Result<TDestination>.NotFound(result.Errors?.ToArray<string>());
                case ResultStatus.Unauthorized: return Result<TDestination>.Unauthorized();
                case ResultStatus.Forbidden: return Result<TDestination>.Forbidden();
                case ResultStatus.Invalid: return Result<TDestination>.Invalid(result.ValidationErrors);
                case ResultStatus.Error: return Result<TDestination>.Error(result.Errors.ToArray());
                default:
                    throw new NotSupportedException($"Result {result.Status} conversion is not supported.");
            }
        }

        public static Result Transform<TSource>(this Result<TSource> result)
        {
            switch (result.Status)
            {
                case ResultStatus.Ok: return Result.Success();
                case ResultStatus.NotFound: return Result.NotFound(result.Errors?.ToArray<string>());
                case ResultStatus.Unauthorized: return Result.Unauthorized();
                case ResultStatus.Forbidden: return Result.Forbidden();
                case ResultStatus.Invalid: return Result.Invalid(result.ValidationErrors);
                case ResultStatus.Error: return Result.Error(result.Errors.ToArray());
                default:
                    throw new NotSupportedException($"Result {result.Status} conversion is not supported.");
            }
        }

        public static string ResultDetails(this IResult result)
        {
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

            details.Append(result.GetValue().ToString());

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

            if (errors.Any())
            {
                details.Append("Next validation error(s) occured: ");

                foreach (var error in errors)
                {
                    details.Append($"Identifier: {error.Key}. Message: {error.Value}");
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