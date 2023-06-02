namespace JordiAragon.SharedKernel.Infrastructure.EventBus
{
    using System;
    using System.Text;

    public class RabbitMqOptions
    {
        public const string Section = "MessageBroker:RabbitMQ";

        public string Host { get; init; }

        public string Username { get; init; }

        public string Password { get; init; }
    }
}