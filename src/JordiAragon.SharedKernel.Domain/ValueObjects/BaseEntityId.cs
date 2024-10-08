﻿namespace JordiAragon.SharedKernel.Domain.ValueObjects
{
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public abstract class BaseEntityId<TIdType> : BaseValueObject, IEntityId<TIdType>
        where TIdType : notnull
    {
        protected BaseEntityId(TIdType value)
        {
            Guard.Against.Default(value, nameof(value));
            Guard.Against.Null(value, nameof(value));

            this.Value = value;
        }

        // Required by EF.
        protected BaseEntityId()
        {
        }

        public TIdType Value { get; init; } = default!;

        public static implicit operator TIdType(BaseEntityId<TIdType> self)
        {
            Guard.Against.Null(self, nameof(self));

            return self.Value;
        }

        public TIdType FromBaseEntityId(BaseEntityId<TIdType> self)
        {
            return self;
        }

        public override string? ToString()
            => this.Value.ToString();

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}