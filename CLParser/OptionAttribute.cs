using System;
using System.Linq;
using System.Reflection;

namespace CLParser
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class OptionAttribute : Attribute
    {
        public string FullName;
        public char ShortName;

        public OptionAttribute(string fullName)
        {
            FullName = fullName;
        }

        public OptionAttribute(int position)
        {

        }

        internal static OptionAttribute FromInfo(FieldInfo info)
        {
            return (OptionAttribute)info.GetCustomAttributes(true)
                .First(attr => attr is OptionAttribute);
        }

        internal static OptionAttribute FromInfo(PropertyInfo info)
        {
            return (OptionAttribute)info.GetCustomAttributes(true)
                .First(attr => attr is OptionAttribute);
        }
    }
}
