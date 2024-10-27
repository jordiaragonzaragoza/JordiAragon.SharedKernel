namespace JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces
{
    public interface ICacheValue<out T>
    {
        bool HasValue { get; }

        bool IsNull { get; }

        T Value { get; }
    }
}