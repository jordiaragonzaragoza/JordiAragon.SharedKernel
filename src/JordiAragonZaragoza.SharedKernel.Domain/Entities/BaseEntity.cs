﻿namespace JordiAragonZaragoza.SharedKernel.Domain.Entities
{
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragonZaragoza.SharedKernel.Contracts.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public abstract class BaseEntity<TId> : IEqualityComparer<BaseEntity<TId>>, IEntity<TId>, IIgnoreDependency
        where TId : class, IEntityId
    {
        protected BaseEntity(TId id)
        {
            this.Id = Guard.Against.Null(id, nameof(id));
        }

        // Required by EF
        protected BaseEntity()
        {
        }

        public TId Id { get; protected set; } = default!;

        public bool Equals(BaseEntity<TId>? x, BaseEntity<TId>? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            {
                return false;
            }

            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(BaseEntity<TId> obj)
        {
            if (obj == null)
            {
                return 0;
            }

            return obj.Id.GetHashCode();
        }

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