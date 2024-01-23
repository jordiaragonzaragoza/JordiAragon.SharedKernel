namespace JordiAragon.SharedKernel.Application.Validators
{
    using FluentValidation;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;

    public abstract class BaseValidator<TRequest> : AbstractValidator<TRequest>, IIgnoreDependency
    {
    }
}