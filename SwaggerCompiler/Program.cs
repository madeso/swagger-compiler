using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwaggerCompiler
{
    using System.IO;

    using YamlDotNet.RepresentationModel;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.NamingConventions;

    class Program
    {
        static void Main(string[] args)
        {
            var filename = args[0];

            try
            {
                var data = File.ReadAllText(filename);
                var input = new StringReader(data);
                var yaml = new YamlStream();
                yaml.Load(input);
                var doc = yaml.Documents[0];

                try
                {
                    var pd = new Swagger(filename);
                    pd.Read(doc.RootNode);
                }
                catch (Exception x)
                {
                    throw new Exception("...while reading " + filename, x);
                }
            }
            catch (Exception xx)
            {
                foreach (var x in GetReversedExceptions(xx))
                {
                    var y = x as YamlError;
                    if (y != null)
                    {
                        Console.WriteLine("{0}({1}): {2}", filename, y.root.Start.Line, y.message);
                    }
                    else
                    {
                        Console.WriteLine("{0}: {1}", filename, x.Message);
                    }
                }
            }

#if DEBUG
            Console.WriteLine("Done.");
            Console.ReadKey();
#endif
        }

        private static IEnumerable<Exception> GetReversedExceptions(Exception xx)
        {
            var exceptions = new List<Exception>();
            var x = xx;
            while (x != null)
            {
                exceptions.Add(x);
                x = x.InnerException;
            }
            exceptions.Reverse();
            return exceptions;
        }
    }
}
