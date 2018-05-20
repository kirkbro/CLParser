using System;
using System.Reflection;

namespace CLParser.Parsing
{
    internal abstract class OptionNode
    {
        internal readonly Type Type;
        internal readonly OptionAttribute Attr;
        internal readonly bool IsFlag;

        protected internal OptionNode(Type option, OptionAttribute attr)
        {
            Type = option;
            Attr = attr;
            IsFlag = option == typeof(bool);
        }

        internal bool Matches(string name)
        {
            return name == Attr.FullName || name == Attr.ShortName.ToString();
        }

        internal abstract void SetValue(object obj, object value);
    }

    internal class FieldOptionNode : OptionNode
    {
        internal readonly FieldInfo Info;

        internal FieldOptionNode(FieldInfo info)
            : base(info.FieldType, OptionAttribute.FromInfo(info))
        {
            Info = info;
        }

        internal override void SetValue(object obj, object value)
        {
            Info.SetValue(obj, value);
        }
    }

    internal class PropertyOptionNode : OptionNode
    {
        internal readonly PropertyInfo Info;

        internal PropertyOptionNode(PropertyInfo info)
            : base(info.PropertyType, OptionAttribute.FromInfo(info))
        {
            Info = info;
        }

        internal override void SetValue(object obj, object value)
        {
            Info.SetValue(obj, value);
        }
    }
}
