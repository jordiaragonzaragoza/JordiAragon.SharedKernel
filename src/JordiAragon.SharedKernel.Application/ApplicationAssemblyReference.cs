namespace JordiAragon.SharedKernel.Application
{
    using System.Reflection;

    public static class ApplicationAssemblyReference
    {
        public static readonly Assembly Assembly = typeof(ApplicationAssemblyReference).Assembly;
    }
}