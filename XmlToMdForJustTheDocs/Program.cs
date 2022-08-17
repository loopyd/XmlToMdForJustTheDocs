using XmlToMdForJustTheDocs.Models;

namespace XmlToMdForJustTheDocs
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var options = new Options();
            options.InputFile = "PrecomputedCulling.xml";
            options.OutputDirectory = "PrecomputedCulling";
            options.Version = "v0.8.1";

            var converter = new XmlClassConverter();
            converter.Generate(converter.Convert(options), options);
        }
    }
}