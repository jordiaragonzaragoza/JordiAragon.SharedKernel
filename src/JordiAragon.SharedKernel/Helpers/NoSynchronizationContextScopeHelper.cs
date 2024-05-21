namespace JordiAragon.SharedKernel.Helpers
{
    using System;
    using System.Threading;

    public static class NoSynchronizationContextScopeHelper
    {
        public static Disposable Enter()
        {
            var context = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(null);
            return new Disposable(context);
        }

        public readonly struct Disposable : IDisposable
        {
            private readonly SynchronizationContext synchronizationContext;

            public Disposable(SynchronizationContext synchronizationContext)
            {
                this.synchronizationContext = synchronizationContext;
            }

            public void Dispose() =>
                SynchronizationContext.SetSynchronizationContext(this.synchronizationContext);
        }
    }
}