using System;
using System.ComponentModel;
using System.Reflection;

namespace Bt.Web.Common
{
    public static class EnumGetDescriptionExtension
    {
        public static string GetDescription(this Enum enumValue)
        {
            Type type = enumValue.GetType();
            FieldInfo info = type.GetField(enumValue.ToString());

            if (info is null)
                return string.Empty;
            
            DescriptionAttribute[] da = (DescriptionAttribute[])(info.GetCustomAttributes(typeof(DescriptionAttribute), false));

            return da.Length > 0 ? da[0].Description : string.Empty; 
        }
    }
}