namespace JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces
{
    public interface IBusinessRule
    {
        string Message { get; }

        bool IsBroken();
    }
}