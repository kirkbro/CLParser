using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CLParser.Parsing
{
    internal class VerbNode
    {
        internal readonly Type Type;
        internal readonly VerbAttribute Attr;
        internal readonly List<OptionNode> OptionNodes;

        internal VerbNode(Type verb)
        {
            Type = verb;
            Attr = VerbAttribute.FromType(verb);
            OptionNodes = new List<OptionNode>();

            // Add member option fields
            foreach (FieldInfo info in verb.GetFields())
                if (info.GetCustomAttributes(true).Any(attr => attr is OptionAttribute))
                    OptionNodes.Add(new FieldOptionNode(info));

            // Add member option properties
            foreach (PropertyInfo info in verb.GetProperties())
                if (info.GetCustomAttributes(true).Any(attr => attr is OptionAttribute))
                    OptionNodes.Add(new PropertyOptionNode(info));
        }

        internal bool Matches(string name)
        {
            return name == Attr.FullName || name == Attr.ShortName.ToString();
        }
    }
}
