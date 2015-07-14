namespace SwaggerCompiler
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Cryptography.X509Certificates;

    using Microsoft.SqlServer.Server;

    using YamlDotNet.Core.Tokens;
    using YamlDotNet.RepresentationModel;

    internal class Swagger
    {
        private readonly string filename;

        private YamlNode root;

        public Swagger(string filename)
        {
            this.filename = filename;
        }

        public void Read(YamlNode rootNode)
        {
            this.root = rootNode;
            if (rootNode.GetRequiredString("swagger") != "2.0")
            {
                throw new YamlError(rootNode, "swagger isn't 2.0");
            }
            ReadPaths(rootNode.FirstChildNamed("paths"));
        }

        private void ReadPaths(YamlNode p)
        {
            foreach (var ch in p.Children())
            {
                var path = ch.Key.AsString();
                try
                {
                    ReadPathItem(ch.Value);
                }
                catch (Exception x)
                {
                    throw new YamlError(x, p, "...while reading path " + path);
                }
            }
        }

        private void ReadPathItem(YamlNode value)
        {
            string[] ass = { "get", "put", "post", "delete", "options", "head", "patch" };
            foreach (var name in ass)
            {
                var c = value.FirstChildNamedOrNull(name);
                if (c != null)
                {
                    try
                    {
                        ReadOperationsObject(c);
                    }
                    catch (Exception x)
                    {
                        throw new YamlError(x, value, "...while reading " + name);
                    }
                }
            }
        }

        private void ReadOperationsObject(YamlNode yamlNode)
        {
            var p = yamlNode.FirstChildNamedOrNull("parameters");
            if( p != null )
                foreach (var x in p.ArrayItems())
                {
                    ReadParametersObject(x);
                }
            ReadResponsesObject(yamlNode.FirstChildNamed("responses"));
        }

        private void ReadResponsesObject(YamlNode re)
        {
            foreach (var r in re.Children())
            {
                this.ReadResponseObject(r.Value);
            }
        }

        private void ReadResponseObject(YamlNode value)
        {
            value.GetRequiredString("description");
            var schema = value.FirstChildNamedOrNull("schema");
            if (schema != null) ReadSchemaObject(schema);
        }

        private void ReadSchemaObject(YamlNode schema)
        {
            try
            {
                var r = schema.GetStringOrNull("$ref");
                if (r != null)
                {
                    try
                    {
                        var node = GetNode(r);
                        this.ReadSchemaObject(node);
                    }
                    catch (Exception x)
                    {
                        throw new YamlError(x, schema, "...while reading ref " + r);
                    }
                }

                var type = schema.GetStringOrNull("type");
                if (type != null)
                {
                    this.IsOneOf(type, "array", "boolean", "integer", "number", "null", "object", "string");
                    if (type == "array")
                    {
                        this.ReadSchemaObject(schema.FirstChildNamed("items"));
                    }
                }

                var properties = schema.FirstChildNamedOrNull("properties");
                if (properties != null)
                {
                    foreach (var prop in properties.Children())
                    {
                        MatchName(prop.Key, prop.Key.AsString());
                        this.ReadSchemaObject(prop.Value);
                    }
                }
            }
            catch (Exception xx)
            {
                throw new YamlError(xx, schema, "...while schema object");
            }
        }

        private void MatchName(YamlNode key, string asString)
        {
            // make sure it maches azAZ09
        }

        private YamlNode GetNode(string path)
        {
            if( path.StartsWith("#/") == false) throw new Exception("Invalid path " + path);
            var sub = path.Substring(2).Split("/".ToCharArray());
            var node = this.root;
            
            foreach (var s in sub)
            {
                try
                {
                    node = node.FirstChildNamed(s);
                }
                catch (Exception xx)
                {
                    throw new YamlError(xx, node, string.Format("...while getting a node from {0}", path));
                }
            }

            return node;
        }

        private void ReadParametersObject(YamlNode par)
        {
            var name = "<unknown>";
            try
            {
                name = par.GetRequiredString("name");
                this.MatchName(par, name);

                var din = par.GetRequiredString("in");
                IsOneOf(din, "query", "header", "path", "formData", "body");
                if (din == "body")
                {
                    this.ReadSchemaObject(par.FirstChildNamed("schema"));
                }
                else
                {
                    par.GetRequiredString("type"); // required
                    this.ReadSchemaObject(par);

                    var form = par.GetStringOrNull("format");
                    if (form != null)
                    {
                        this.IsOneOf(form, "int32", "int64", "float", "double", "byte", "date", "date-time", "password");
                    }
                }
                
                this.ReadSchemaObject(par);
            }
            catch (Exception xx)
            {
                throw new YamlError(xx, par, "...while reading parameter " + name);
            }
        }

        private void IsOneOf(string s, params string[] arr)
        {
            foreach (var a in arr)
            {
                if (s == a) return;
            }

            throw new Exception(string.Format("{0} is not a valid value, valid are {1}", s, StringListCombiner.EnglishOr.CombineFromEnumerable(arr)));
        }
    }
}