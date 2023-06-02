namespace JordiAragon.SharedKernel.Infrastructure.Cache
{
    public class CacheOptions
    {
        public const string Section = "Cache";

        public string DefaultName { get; set; }

        public int DefaultExpirationInSeconds { get; set; }
    }
}