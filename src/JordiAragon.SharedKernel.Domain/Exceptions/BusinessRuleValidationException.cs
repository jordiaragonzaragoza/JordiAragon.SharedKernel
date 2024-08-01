namespace JordiAragon.SharedKernel.Domain.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    [Serializable]
    public class BusinessRuleValidationException : Exception
    {
        public BusinessRuleValidationException(IBusinessRule brokenRule)
            : base(brokenRule.Message)
        {
            this.BrokenRule = Guard.Against.Null(brokenRule, nameof(brokenRule));
        }

        protected BusinessRuleValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public IBusinessRule BrokenRule { get; } = default!;

        public override string ToString()
        {
            return $"{this.BrokenRule.GetType().FullName}: {this.BrokenRule.Message}";
        }
    }
}