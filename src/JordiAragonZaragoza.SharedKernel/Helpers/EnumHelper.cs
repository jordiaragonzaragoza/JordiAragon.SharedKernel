namespace JordiAragonZaragoza.SharedKernel.Helpers
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;

    public static class EnumHelper
    {
        public static string GetEnumMemberValue<TEnum>(TEnum value)
            where TEnum : Enum
        {
            var type = typeof(TEnum);
            var memberInfo = type.GetMember(value.ToString()).FirstOrDefault();
            var attribute = memberInfo?.GetCustomAttribute<EnumMemberAttribute>();
            return attribute?.Value ?? string.Empty;
        }
    }
}