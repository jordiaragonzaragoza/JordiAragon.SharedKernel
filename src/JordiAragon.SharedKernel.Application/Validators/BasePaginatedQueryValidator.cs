namespace JordiAragon.SharedKernel.Application.Validators
{
    using FluentValidation;
    using JordiAragon.SharedKernel.Application.Contracts;

    public abstract class BasePaginatedQueryValidator<TQuery> : BaseValidator<TQuery>
        where TQuery : IPaginatedQuery
    {
        protected BasePaginatedQueryValidator()
        {
            this.RuleFor(x => x.PageNumber)
                .Must(pageNumber => pageNumber >= 0)
                .WithMessage("PageNumber must be greater than or equal to 0.");

            this.RuleFor(x => x.PageSize)
                .Must(pageSize => pageSize >= 0)
                .WithMessage("PageSize must be greater than or equal to 0.");
        }
    }
}