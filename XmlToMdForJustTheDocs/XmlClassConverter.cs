using System.Text;
using System.Xml.Serialization;
using XmlToMdForJustTheDocs.XmlTypes;

namespace XmlToMdForJustTheDocs;

public class XmlClassConverter
{
    public Documentation Convert(Options options)
    {
        var serializer = new XmlSerializer(typeof(Documentation));
        using var reader = File.OpenText(options.InputFile);
        var documentation = (Documentation)serializer.Deserialize(reader);
        return documentation;
    }

    public void Generate(Documentation documentation, Options options)
    {
        var outputFolder = new DirectoryInfo(options.OutputDirectory);

        var versionFolder = outputFolder.CreateChildFolder(options.Version);
        var assemblyFolder = versionFolder.CreateChildFolder(documentation.Assembly.Name);
        var md = new MdGenerator();

        foreach (var member in documentation.Members.MembersList)
        {
            var jtd = new Jtd
            {
                Title = member.Namespace + "." + member.MemberName,
            };
            md.JtdToHeader(jtd);
            md.H1(member.MemberName);
            md.H4($"{member.TypeName}: {member.MemberName}");
            md.H5($"Namespace: {member.Namespace}");
            md.AppendLine();
            md.H3("Summary");
            if (member.Summary != null)
            {
                md.IndentText(member.Summary.Text);

                if (member.Summary.Remarks.Count != 0)
                {
                    md.H3("Remarks");
                    foreach (var remark in member.Summary.Remarks)
                    {
                        md.IndentText(remark);
                    }
                }
            }
        }
    }
}

public class Jtd
{
    public string Layout { get; set; } = "default";
    public string? Title { get; set; }
    public int? NavigationOrder { get; set; }
    public string? Permalink { get; set; }
    public bool? HasChildren { get; set; }
}

public static class FileInfoUtils
{
    public static bool IsDirectory(this FileSystemInfo path)
    {
        return path is DirectoryInfo;
    }

    public static bool IsFile(this FileSystemInfo path)
    {
        return path is not DirectoryInfo;
    }

    public static FileSystemInfo CreateChildFolder(this FileSystemInfo root, string folder)
    {
        if (!root.IsDirectory())
        {
            throw new ArgumentException("provided FileSystemInfo was not a directory but a file");
        }

        return Directory.CreateDirectory(root.FullName + "/" + folder);
    }
}