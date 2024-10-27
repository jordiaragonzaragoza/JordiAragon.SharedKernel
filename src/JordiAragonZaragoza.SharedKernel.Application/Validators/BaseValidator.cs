namespace JordiAragonZaragoza.SharedKernel.Application.Validators
{
    using FluentValidation;
    using JordiAragonZaragoza.SharedKernel.Contracts.DependencyInjection;

    public abstract class BaseValidator<TRequest> : AbstractValidator<TRequest>, IIgnoreDependency
    {
    }
}