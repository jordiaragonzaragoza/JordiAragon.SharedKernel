namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    /// <summary>
    /// It indicates whether is logically deleted.
    /// </summary>
    public interface ISoftDelete // TODO: Complete implementation.
    {
        bool IsDeleted { get; }
    }
}