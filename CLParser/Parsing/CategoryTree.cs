using System;
using System.Collections.Generic;
using System.Linq;

namespace CLParser.Parsing
{
    internal class CategoryTree
    {
        internal readonly Type Type;
        internal readonly CategoryAttribute Attr;
        internal readonly List<CategoryTree> Children;
        internal readonly List<VerbNode> VerbNodes;

        internal CategoryTree(Type category)
        {
            Type = category;
            Attr = CategoryAttribute.FromType(category);
            Children = new List<CategoryTree>();
            VerbNodes = new List<VerbNode>();
        }

        internal void AddChild(CategoryTree node) => Children.Add(node);
        internal void AddVerbNode(VerbNode node) => VerbNodes.Add(node);

        internal CategoryTree Find(Type category)
        {
            if (category == Type)
                return this;

            return Children
                .Select(child => child.Find(category))
                .FirstOrDefault(node => node != null);
        }

        internal bool Matches(string name)
        {
            return name == Attr.FullName || name == Attr.ShortName.ToString();
        }
    }
}
