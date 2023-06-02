namespace JordiAragon.SharedKernel.Domain
{
    using System.Reflection;

    public static class DomainAssemblyReference
    {
        public static readonly Assembly Assembly = typeof(DomainAssemblyReference).Assembly;
    }
}