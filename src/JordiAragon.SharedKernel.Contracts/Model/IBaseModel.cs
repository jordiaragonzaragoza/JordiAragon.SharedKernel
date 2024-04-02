namespace JordiAragon.SharedKernel.Contracts.Model
{
    /// <summary>
    /// Generic abstraction for a base model.
    /// </summary>
    /// <typeparam name="TId">The id for the base model.</typeparam>
    public interface IBaseModel<out TId>
        where TId : notnull
    {
        TId Id { get; }
    }
}