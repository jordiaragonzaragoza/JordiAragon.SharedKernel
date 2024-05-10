namespace JordiAragon.SharedKernel.Contracts.Model
{
    public interface IOptimisticLockable
    {
        byte[] Version { get; }
    }
}