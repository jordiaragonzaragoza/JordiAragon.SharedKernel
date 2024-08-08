namespace JordiAragon.SharedKernel.Infrastructure.ExternalBus
{
    public class RabbitMqOptions
    {
        public const string Section = "MessageBroker:RabbitMQ";

        public string Host { get; init; } = default!;

        public string Username { get; init; } = default!;

        public string Password { get; init; } = default!;
    }
}