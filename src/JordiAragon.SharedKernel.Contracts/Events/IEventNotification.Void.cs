namespace JordiAragon.SharedKernel.Contracts.Events
{
    using System;
    using MediatR;

    public interface IEventNotification : INotification
    {
        public Guid Id { get; }
    }
}