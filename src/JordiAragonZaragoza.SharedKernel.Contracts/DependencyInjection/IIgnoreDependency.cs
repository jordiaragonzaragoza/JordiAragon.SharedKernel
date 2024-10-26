namespace JordiAragonZaragoza.SharedKernel.Contracts.DependencyInjection
{
    /// <summary>
    /// Specifies that a type in a scanned assembly will not be automatically registered.
    /// If not specified with this interface it will be set as a transient dependency.
    /// </summary>
    public interface IIgnoreDependency
    {
    }
}