namespace XmlToMdForJustTheDocs;

public class Options
{
    public string InputFile { get; set; }
    public string OutputDirectory { get; set; }
    public string Version { get; set; }
}

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