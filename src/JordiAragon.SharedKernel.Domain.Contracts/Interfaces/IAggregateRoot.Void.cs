namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    // Apply this marker interface only to aggregate root entities
    // Write repositories will only work with aggregate roots, not their children
    public interface IAggregateRoot
    {
    }
}