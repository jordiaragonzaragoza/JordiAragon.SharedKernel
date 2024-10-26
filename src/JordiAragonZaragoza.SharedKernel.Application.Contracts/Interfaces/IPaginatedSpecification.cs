namespace JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces
{
    using Ardalis.Specification;

    public interface IPaginatedSpecification<TReadModel> : ISpecification<TReadModel>
    {
        IPaginatedQuery Request { get; }
    }
}