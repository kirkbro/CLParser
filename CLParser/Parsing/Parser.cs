using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CLParser.Parsing
{
    public class Parser
    {
        [Category("")]
        private class RootCategory {}

        private CategoryTree root = new CategoryTree(typeof(RootCategory));
        private Dictionary<Type, Delegate> typeParsers = new Dictionary<Type, Delegate>();

        public Parser() {}

        public static Parser Create()
        {
            return new Parser();
        }

        public Parser AddVerb<T>(Action<T> handler)
        {
            // Recurse declaring types tagged with CategoryAttribute
            List<Type> lineage = GetCategoryLineage(typeof(T));

            // Find deepest common category ancestor between T and root
            CategoryTree common = root;
            int i = 0;
            for (; i < lineage.Count; i++)
            {
                CategoryTree next = common.Find(lineage[i]);
                if (next == null)
                    break;
                common = next;
            }

            // Create missing nodes in category tree
            for (; i < lineage.Count; i++)
            {
                CategoryTree node = new CategoryTree(lineage[i]);
                common.AddChild(node);
                common = node;
            }

            // Add verb to the deepest category
            common.AddVerbNode(new VerbNode(typeof(T)));

            return this;
        }

        public Parser AddParser<T>(Func<string, T> parser)
        {
            typeParsers[typeof(T)] = parser;
            return this;
        }

        public void Process(string[] args)
        {
            // Find deepest matching category
            CategoryTree catNode = root;
            int i = 0;
            for (; i < args.Length; i++)
            {
                CategoryTree child = catNode.Children
                    .FirstOrDefault(c => c.Matches(args[i]));

                if (child == null)
                    break;

                catNode = child;
            }

            // Find matching verb in category
            VerbNode verbNode = catNode.VerbNodes
                .First(v => v.Matches(args[i]));
            i++;

            // Remove categories and verb from argument list
            args = args.Slice(i);

            // Construct result verb object
            object verbObj = Activator.CreateInstance(verbNode.Type);

            while (true)
            {
                (int oi, string name) = FindLongOption(args);
                if (oi == -1)
                    break;

                OptionNode optionNode = verbNode.OptionNodes
                    .First(o => o.Matches(name));

                if (optionNode.IsFlag)
                {
                    // Remove option from args
                    (string _, string[] rest) = RemoveLongOption(args, oi, false);
                    args = rest;

                    optionNode.SetValue(verbObj, true);
                }
                else
                {
                    // Remove option from args
                    (string arg, string[] rest) = RemoveLongOption(args, oi, true);
                    args = rest;

                    object value = typeParsers[optionNode.Type].DynamicInvoke(arg);
                    optionNode.SetValue(verbObj, value);
                }
            }
        }

        private static (int, string) FindLongOption(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                Match match = Regex.Match(args[i], "--(?<name>.+)");
                if (match.Success)
                    return (i, match.Groups["name"].Value);
            }

            return (-1, null);
        }

        private static (string, string[]) RemoveLongOption(string[] args, int i, bool withArg)
        {
            return withArg
                ? (args[i + 1], args.Delete(i, i + 2))
                : (null, args.Delete(i, i + 1));
        }

        private static List<Type> GetCategoryLineage(Type type)
        {
            List<Type> ancestry = new List<Type>();
            Type category = type.DeclaringType;
            while (category != null && CategoryAttribute.IsOn(category))
            {
                ancestry.Add(category);
                category = category.DeclaringType;
            }

            // From old to young
            ancestry.Reverse();
            return ancestry;
        }
    }
}
