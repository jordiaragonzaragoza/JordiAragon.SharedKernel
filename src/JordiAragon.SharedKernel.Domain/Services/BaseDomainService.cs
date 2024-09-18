namespace JordiAragon.SharedKernel.Domain.Services
{
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Exceptions;

    public abstract class BaseDomainService : IDomainService
    {
        protected static void CheckRule(IBusinessRule rule)
        {
            Guard.Against.Null(rule, nameof(rule));

            if (rule.IsBroken())
            {
                throw new BusinessRuleValidationException(rule);
            }
        }
    }
}