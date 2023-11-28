namespace JordiAragon.SharedKernel.Domain.Entities
{
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Exceptions;

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

        public TId Id { get; protected set; }

        public bool Equals(BaseEntity<TId> x, BaseEntity<TId> y)
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

        public int GetHashCode(BaseEntity<TId> entity)
        {
            if (entity == null)
            {
                return 0;
            }

            return entity.Id.GetHashCode();
        }

        protected static void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken())
            {
                throw new BusinessRuleValidationException(rule);
            }
        }
    }
}