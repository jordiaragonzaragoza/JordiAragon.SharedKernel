namespace JordiAragon.SharedKernel.Contracts.Model
{
    public interface IOptimisticLockable
    {
        uint Version { get; }
    }
}