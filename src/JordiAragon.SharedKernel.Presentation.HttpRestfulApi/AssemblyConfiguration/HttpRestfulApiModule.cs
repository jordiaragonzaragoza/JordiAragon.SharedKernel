namespace JordiAragon.SharedKernel.Presentation.HttpRestfulApi.AssemblyConfiguration
{
    using System.Reflection;
    using JordiAragon.SharedKernel;

    public class HttpRestfulApiModule : AssemblyModule
    {
        protected override Assembly CurrentAssembly => HttpRestfulApiAssemblyReference.Assembly;
    }
}