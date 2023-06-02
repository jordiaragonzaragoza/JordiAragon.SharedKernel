namespace JordiAragon.SharedKernel.Infrastructure
{
    using System.Reflection;

    public static class InfrastructureAssemblyReference
    {
        public static readonly Assembly Assembly = typeof(InfrastructureAssemblyReference).Assembly;
    }
}