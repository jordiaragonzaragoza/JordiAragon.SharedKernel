﻿namespace JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers
{
    using System;
    using System.Linq;
    using System.Text;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Extensions to support converting Result to an ActionResult.
    /// Based on <see href="https://github.com/ardalis/Result/blob/main/src/Ardalis.Result.AspNetCore/ResultExtensions.cs">Ardalis.Result.AspNetCore</see>.
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Convert a <see cref="Result{T}"/> to a <see cref="ActionResult"/>.
        /// </summary>
        /// <typeparam name="T">The value being returned.</typeparam>
        /// <param name="result">The Result to convert to an ActionResult.</param>
        /// <param name="controller">The controller this is called from.</param>
        /// <returns>The <see cref="ActionResult{T}"/> converted.</returns>
        public static ActionResult<T> ToActionResult<T>(this Result<T> result, ControllerBase controller)
        {
            Guard.Against.Null(result, nameof(result));
            Guard.Against.Null(controller, nameof(controller));

            return controller.ToActionResult((IResult)result);
        }

        /// <summary>
        /// Convert a <see cref="Result"/> to a <see cref="ActionResult"/>.
        /// </summary>
        /// <param name="result">The Result to convert to an ActionResult.</param>
        /// <param name="controller">The controller this is called from.</param>
        /// <returns>The <see cref="ActionResult"/> converted.</returns>
        public static ActionResult ToActionResult(this Result result, ControllerBase controller)
        {
            Guard.Against.Null(result, nameof(result));
            Guard.Against.Null(controller, nameof(controller));

            return controller.ToActionResult((IResult)result);
        }

        /// <summary>
        /// Convert a <see cref="Result{T}"/> to a <see cref="ActionResult"/>.
        /// </summary>
        /// <typeparam name="T">The value being returned.</typeparam>
        /// <param name="controller">The controller this is called from.</param>
        /// <param name="result">The Result to convert to an ActionResult.</param>
        /// <returns>The <see cref="ActionResult{T}"/> converted.</returns>
        public static ActionResult<T> ToActionResult<T>(this ControllerBase controller, Result<T> result)
        {
            Guard.Against.Null(result, nameof(result));
            Guard.Against.Null(controller, nameof(controller));

            return controller.ToActionResult((IResult)result);
        }

        /// <summary>
        /// Convert a <see cref="Result"/> to a <see cref="ActionResult"/>.
        /// </summary>
        /// <param name="controller">The controller this is called from.</param>
        /// <param name="result">The Result to convert to an ActionResult.</param>
        /// <returns>The <see cref="ActionResult"/> converted.</returns>
        public static ActionResult ToActionResult(this ControllerBase controller, Result result)
        {
            Guard.Against.Null(result, nameof(result));
            Guard.Against.Null(controller, nameof(controller));

            return controller.ToActionResult((IResult)result);
        }

        /// <summary>
        /// Convert a <see cref="Result{FileResponse}"/> to a <see cref="ActionResult"/>.
        /// </summary>
        /// <param name="result">The Result{FileResponse} to convert to an ActionResult.</param>
        /// <param name="controller">The controller this is called from.</param>
        /// <returns>The <see cref="ActionResult"/> converted.</returns>
        public static ActionResult ToFileResult(this Result<FileResponse> result, ControllerBase controller)
        {
            return controller.ToFileResult(result);
        }

        /// <summary>
        /// Convert a <see cref="Result"/> to a <see cref="ActionResult"/>.
        /// </summary>
        /// <param name="controller">The controller this is called from.</param>
        /// <param name="result">The Result{FileResponse} to convert to an ActionResult.</param>
        /// <returns>The <see cref="ActionResult"/> converted.</returns>
        public static ActionResult ToFileResult(this ControllerBase controller, Result<FileResponse> result)
        {
            Guard.Against.Null(result, nameof(result));
            Guard.Against.Null(controller, nameof(controller));

            switch (result.Status)
            {
                case ResultStatus.Ok:
                    return controller.File(result.Value.FileContents, result.Value.ContentType, result.Value.FileDownloadName);
                case ResultStatus.NotFound: return NotFoundEntity(controller, result);
                case ResultStatus.Unauthorized: return Unauthorized(controller);
                case ResultStatus.Forbidden: return Forbidden(controller);
                case ResultStatus.Invalid: return BadRequest(controller, result);
                case ResultStatus.Error: return UnprocessableEntity(controller, result);
                default:
                    throw new NotSupportedException($"Result {result.Status} conversion is not supported.");
            }
        }

        internal static ActionResult ToActionResult(this ControllerBase controller, IResult result)
        {
            switch (result.Status)
            {
                case ResultStatus.Ok:
                    return (result is Result) ? (ActionResult)controller.Ok()
                        : controller.Ok(result.GetValue());
                case ResultStatus.NotFound: return NotFoundEntity(controller, result);
                case ResultStatus.Unauthorized: return Unauthorized(controller);
                case ResultStatus.Forbidden: return Forbidden(controller);
                case ResultStatus.Invalid: return BadRequest(controller, result);
                case ResultStatus.Error: return UnprocessableEntity(controller, result);
                default:
                    throw new NotSupportedException($"Result {result.Status} conversion is not supported.");
            }
        }

        private static UnauthorizedObjectResult Unauthorized(ControllerBase controller)
        {
            var problemDetails = new ProblemDetails
            {
                Instance = controller.HttpContext.Request.Path,
                Type = "https://www.rfc-editor.org/rfc/rfc7235#section-3.1",
                Title = "Unauthorized.",
            };

            return controller.Unauthorized(problemDetails);
        }

        private static BadRequestObjectResult BadRequest(ControllerBase controller, IResult result)
        {
            var errors = result.ValidationErrors
                .GroupBy(x => x.Identifier, x => x.ErrorMessage)
                .ToDictionary(g => g.Key, g => g.ToArray());

            return controller.BadRequest(new ValidationProblemDetails(errors)
            {
                Instance = controller.HttpContext.Request.Path,
                Type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.1",
                Title = "Bad Request.",
                Detail = "Please refer to the errors property for additional details.",
            });
        }

        private static UnprocessableEntityObjectResult UnprocessableEntity(ControllerBase controller, IResult result)
        {
            var details = new StringBuilder("Next error(s) occured: ");

            foreach (var error in result.Errors)
            {
                details.Append(error);
            }

            return controller.UnprocessableEntity(new ProblemDetails
            {
                Instance = controller.HttpContext.Request.Path,
                Type = "https://www.rfc-editor.org/rfc/rfc9110#name-422-unprocessable-content",
                Title = "Something went wrong.",
                Detail = details.ToString(),
            });
        }

        private static NotFoundObjectResult NotFoundEntity(ControllerBase controller, IResult result)
        {
            var problemDetails = new ProblemDetails
            {
                Instance = controller.HttpContext.Request.Path,
                Type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.4",
                Title = "Resource not found.",
            };

            if (result.Errors.Any())
            {
                var details = new StringBuilder("Next error(s) occured: ");

                foreach (var error in result.Errors)
                {
                    details.Append(error);
                }

                problemDetails.Detail = details.ToString();

                return controller.NotFound(problemDetails);
            }

            return controller.NotFound(problemDetails);
        }

        private static ForbidResult Forbidden(ControllerBase controller)
        {
            return controller.Forbid();
        }
    }
}