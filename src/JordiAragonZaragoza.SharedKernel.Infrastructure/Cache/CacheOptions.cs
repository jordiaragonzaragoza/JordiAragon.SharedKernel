namespace JordiAragonZaragoza.SharedKernel.Infrastructure.Cache
{
    public class CacheOptions
    {
        public const string Section = "Cache";

        public string DefaultName { get; set; } = default!;

        public int DefaultExpirationInSeconds { get; set; }
    }
}