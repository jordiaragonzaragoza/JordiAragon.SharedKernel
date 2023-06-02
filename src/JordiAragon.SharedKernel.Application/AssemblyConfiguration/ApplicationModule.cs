namespace JordiAragon.SharedKernel.Application.AssemblyConfiguration
{
    using System.Reflection;
    using JordiAragon.SharedKernel;

    public class ApplicationModule : AssemblyModule
    {
        protected override Assembly CurrentAssembly => ApplicationAssemblyReference.Assembly;
    }
}