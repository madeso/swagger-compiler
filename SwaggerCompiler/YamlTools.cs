namespace SwaggerCompiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using YamlDotNet.RepresentationModel;

    internal static class YamlTools
    {
        public static string GetRequiredString(this YamlNode n, string name)
        {
            var node = FirstChildNamed(n, name);
            return node.AsString();
        }

        public static string GetStringOrNull(this YamlNode n, string name)
        {
            var node = FirstChildNamedOrNull(n, name);
            if( node == null ) return null;
            return node.AsString();
        }

        public static IDictionary<YamlNode, YamlNode> Children(this YamlNode n)
        {
            var nn = n as YamlMappingNode;
            if( nn == null ) throw new YamlError(n, "Not a mapping node");
            return nn.Children;
        }

        public static YamlScalarNode AsScalar(this YamlNode n)
        {
            return n as YamlScalarNode;
        }

        public static string AsString(this YamlNode n)
        {
            var s = n.AsScalar();
            return s.Value;
        }

        public static IEnumerable<YamlNode> ChildrenNamed(this YamlNode n, string name)
        {
            foreach (var child in n.Children())
            {
                if( child.Key.AsString() == name )
                    yield return child.Value;
            }
        }

        public static IEnumerable<YamlNode> ArrayItems(this YamlNode n)
        {
            var arr = n as YamlSequenceNode;
            if (arr == null) throw new YamlError(n, "Not a array node");
            return arr.Children;
        }

        public static YamlNode FirstChildNamed(this YamlNode n, string name)
        {
            var list = n.ChildrenNamed(name).ToList();
            if (list.Count != 1) throw new YamlError(n, string.Format("Found {0} nodes named {1}, expected 1", list.Count, name));
            return list[0];
        }

        public static YamlNode FirstChildNamedOrNull(this YamlNode n, string name)
        {
            var list = n.ChildrenNamed(name).ToList();
            if (list.Count != 1) return null;
            return list[0];
        }
    }
}