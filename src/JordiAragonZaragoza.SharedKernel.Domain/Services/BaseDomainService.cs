namespace JordiAragonZaragoza.SharedKernel.Domain.Services
{
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Domain.Exceptions;

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