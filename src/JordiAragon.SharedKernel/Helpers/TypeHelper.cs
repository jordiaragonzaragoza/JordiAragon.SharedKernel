namespace JordiAragon.SharedKernel.Helpers
{
    using System;
    using System.Linq;

    public static class TypeHelper
    {
        public static Type GetFirstMatchingTypeFromCurrentDomainAssembly(string typeName)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(x => x.FullName == typeName || x.Name == typeName))
                .FirstOrDefault() ?? throw new InvalidOperationException($"No type found with the name '{typeName}' in the current application domain.");
        }
    }
}