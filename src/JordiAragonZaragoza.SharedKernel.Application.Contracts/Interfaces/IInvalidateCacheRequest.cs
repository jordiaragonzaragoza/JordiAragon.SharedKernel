namespace JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces
{
    public interface IInvalidateCacheRequest
    {
        public string PrefixCacheKey { get; }
    }
}