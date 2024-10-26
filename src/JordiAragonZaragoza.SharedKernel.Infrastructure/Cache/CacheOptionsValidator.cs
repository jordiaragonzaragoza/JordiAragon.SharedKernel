namespace JordiAragonZaragoza.SharedKernel.Infrastructure.Cache
{
    using FluentValidation;

    public class CacheOptionsValidator : AbstractValidator<CacheOptions>
    {
        public CacheOptionsValidator()
        {
            this.RuleFor(x => x.DefaultName)
                .NotEmpty();

            this.RuleFor(x => x.DefaultExpirationInSeconds)
                .GreaterThan(0);
        }
    }
}