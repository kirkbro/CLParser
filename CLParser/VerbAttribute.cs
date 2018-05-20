using System;
using System.Linq;

namespace CLParser
{
    public class VerbAttribute : Attribute
    {
        public string FullName;
        public char ShortName;

        public VerbAttribute(string fullName)
        {
            FullName = fullName;
        }

        internal static VerbAttribute FromType(Type type)
        {
            return (VerbAttribute)type.GetCustomAttributes(true)
                .First(attr => attr is VerbAttribute);
        }
    }
}
