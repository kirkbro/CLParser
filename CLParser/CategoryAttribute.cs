using System;
using System.Linq;

namespace CLParser
{
    public class CategoryAttribute : Attribute
    {
        public string FullName;
        public char ShortName;

        public CategoryAttribute(string fullName)
        {
            FullName = fullName;
        }

        internal static CategoryAttribute FromType(Type type)
        {
            return (CategoryAttribute)type.GetCustomAttributes(true)
                .First(attr => attr is CategoryAttribute);
        }

        internal static bool IsOn(Type type)
        {
            return type.GetCustomAttributes(true)
                .Any(attr => attr is CategoryAttribute);
        }
    }
}
