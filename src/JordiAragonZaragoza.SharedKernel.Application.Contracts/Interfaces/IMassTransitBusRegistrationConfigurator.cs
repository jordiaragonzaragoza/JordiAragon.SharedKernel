namespace JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces
{
    using System;
    using MassTransit;

    public interface IMassTransitBusRegistrationConfigurator
    {
        Action<IBusRegistrationConfigurator> Configure { get; set; }
    }
}