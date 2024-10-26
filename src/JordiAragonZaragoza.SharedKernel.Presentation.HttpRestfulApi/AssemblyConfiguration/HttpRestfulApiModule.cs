namespace JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.AssemblyConfiguration
{
    using System.Reflection;
    using JordiAragonZaragoza.SharedKernel;

    public class HttpRestfulApiModule : AssemblyModule
    {
        protected override Assembly CurrentAssembly => HttpRestfulApiAssemblyReference.Assembly;
    }
}