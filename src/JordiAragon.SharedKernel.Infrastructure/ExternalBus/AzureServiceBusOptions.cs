namespace JordiAragon.SharedKernel.Infrastructure.ExternalBus
{
    using System.Text;

    public class AzureServiceBusOptions
    {
        public const string Section = "MessageBroker:AzureServiceBus";

        public string Endpoint { get; set; }

        public string SharedAccessKeyName { get; set; }

        public string SharedAccessKey { get; set; }

        public string BuildConnectionString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append($"Endpoint={this.Endpoint};");
            stringBuilder.Append($"SharedAccessKeyName={this.SharedAccessKeyName};");
            stringBuilder.Append($"SharedAccessKey={this.SharedAccessKey};");

            return stringBuilder.ToString();
        }
    }
}