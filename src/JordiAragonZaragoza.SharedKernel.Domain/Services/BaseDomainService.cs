namespace JordiAragonZaragoza.SharedKernel.Domain.Services
{
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public abstract class BaseDomainService : IDomainService
    {
        protected static Result CheckRule(IBusinessRule rule)
        {
            Guard.Against.Null(rule, nameof(rule));

            if (!rule.IsBroken())
            {
                return Result.Success();
            }

            var errors = new List<ValidationError>()
            {
                new()
                {
                    ErrorMessage = rule.Message,
                    Identifier = rule.GetType().Name,
                    Severity = ValidationSeverity.Error,
                },
            };

            return Result.Invalid(errors);
        }
    }
}