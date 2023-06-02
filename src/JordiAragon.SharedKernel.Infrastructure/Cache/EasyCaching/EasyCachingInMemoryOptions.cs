namespace JordiAragon.SharedKernel.Infrastructure.Cache.EasyCaching
{
    public class EasyCachingInMemoryOptions
    {
        public const string Section = "Cache:Easycaching:Inmemory";

        public int MaxRdSecond { get; init; }

        public bool EnableLogging { get; init; }

        public int LockMs { get; init; }

        public int SleepMs { get; init; }

        public int DBConfigSizeLimit { get; init; }

        public int DBConfigExpirationScanFrequency { get; init; }

        public bool DBConfigEnableReadDeepClone { get; init; }

        public bool DBConfigEnableWriteDeepClone { get; init; }
    }
}