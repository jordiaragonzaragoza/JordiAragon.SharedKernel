namespace JordiAragon.SharedKernel.Application.Helpers
{
    using System;
    using System.Linq;
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
    }
}