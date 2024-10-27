namespace JordiAragonZaragoza.SharedKernel.Domain.AssemblyConfiguration
{
    using System.Reflection;
    using JordiAragonZaragoza.SharedKernel;

    public class DomainModule : AssemblyModule
    {
        protected override Assembly CurrentAssembly => DomainAssemblyReference.Assembly;
    }
}