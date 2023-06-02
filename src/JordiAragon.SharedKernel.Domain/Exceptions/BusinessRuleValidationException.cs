namespace JordiAragon.SharedKernel.Domain.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    [Serializable]
    public class BusinessRuleValidationException : Exception
    {
        public BusinessRuleValidationException(IBusinessRule brokenRule)
            : base(brokenRule.Message)
        {
            this.BrokenRule = brokenRule;
            this.Details = brokenRule.Message;
        }

        protected BusinessRuleValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public IBusinessRule BrokenRule { get; init; }

        public string Details { get; init; }

        public override string ToString()
        {
            return $"{this.BrokenRule.GetType().FullName}: {this.BrokenRule.Message}";
        }
    }
}