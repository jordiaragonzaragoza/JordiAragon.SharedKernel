namespace JordiAragon.SharedKernel.Infrastructure.InternalBus
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using global::MediatR;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public class CommandBus : ICommandBus
    {
        private readonly ISender sender;

        public CommandBus(ISender sender)
        {
            this.sender = sender ?? throw new ArgumentNullException(nameof(sender));
        }

        public Task<Result> SendAsync(ICommand command, CancellationToken cancellationToken = default)
            => this.sender.Send(command, cancellationToken);

        public Task<Result<TResponse>> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
            where TResponse : notnull
            => this.sender.Send(command, cancellationToken);
    }
}