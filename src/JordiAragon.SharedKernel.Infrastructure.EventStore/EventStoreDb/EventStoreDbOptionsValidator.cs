namespace JordiAragon.SharedKernel.Infrastructure.EventStore.EventStoreDb
{
    using FluentValidation;

    public class EventStoreDbOptionsValidator : AbstractValidator<EventStoreDbOptions>
    {
        public EventStoreDbOptionsValidator()
        {
            this.RuleFor(x => x.ConnectionString)
                .NotEmpty();
        }
    }
}