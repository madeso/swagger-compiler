namespace SwaggerCompiler
{
    using System;

    using YamlDotNet.RepresentationModel;

    internal class YamlError : Exception
    {
        public string message;
        public YamlNode root;

        public YamlError(YamlNode rootNode, string swaggerIsnT)
            : base(swaggerIsnT)
        {
            this.root = rootNode;
            this.message = swaggerIsnT;
        }

        public YamlError(Exception x, YamlNode rootNode, string swaggerIsnT)
            : base(swaggerIsnT, x)
        {
            this.root = rootNode;
            this.message = swaggerIsnT;
        }
    }
}