namespace JordiAragonZaragoza.SharedKernel.Infrastructure.ExternalBus
{
    using FluentValidation;

    public class RabbitMqOptionsValidator : AbstractValidator<RabbitMqOptions>
    {
        public RabbitMqOptionsValidator()
        {
            this.RuleFor(x => x.Host)
                .NotEmpty();

            this.RuleFor(x => x.Username)
                .NotEmpty();

            this.RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}