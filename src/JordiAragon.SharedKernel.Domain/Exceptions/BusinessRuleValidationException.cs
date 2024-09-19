namespace JordiAragon.SharedKernel.Domain.Exceptions
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    // TODO: Remove and use result pattern.
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Temporal suppresion")]
    public class BusinessRuleValidationException : Exception
    {
        public BusinessRuleValidationException(IBusinessRule brokenRule)
            : base(brokenRule?.Message)
        {
            this.BrokenRule = Guard.Against.Null(brokenRule, nameof(brokenRule));
        }

        public IBusinessRule BrokenRule { get; }

        public override string ToString()
        {
            return $"{this.BrokenRule.GetType().FullName}: {this.BrokenRule.Message}";
        }
    }
}