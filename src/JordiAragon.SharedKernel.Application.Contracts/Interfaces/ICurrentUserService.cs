namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; set; } // TODO: Temporal. Only a get is required.

        bool IsAuthenticated { get; }
    }
}