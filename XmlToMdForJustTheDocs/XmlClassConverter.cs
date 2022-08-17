using System.Text;
using System.Xml.Serialization;
using XmlToMdForJustTheDocs.XmlTypes;
using XmlToMdForJustTheDocs.Models;
using XmlToMdForJustTheDocs.Utils;

namespace XmlToMdForJustTheDocs
{
    public class XmlClassConverter
    {
        #region Public Methods
        public Documentation Convert(Options options)
        {
            var serializer = new XmlSerializer(typeof(Documentation));
            Documentation documentation = new Documentation();
            using (var reader = File.OpenText(options.InputFile)) {
                var result = serializer.Deserialize(reader);
#pragma warning disable CS8604  // Helper Utility tests cast isn't null.
                if (TypeUtilities.TryCast<Documentation>(result, out var documentation2))
#pragma warning restore CS8604  // Helper Utility tests cast isn't null.
                {
                    documentation = documentation2;
                    foreach (var member in documentation.Members.MembersList)
                    {
                        member.InitializeMember();
                    }
                    var a = documentation.Members.TypeMembers.Value;
                }
            }
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
        #endregion
    }

    public static class FileInfoUtils
    {
        #region Public Methods
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
        #endregion
    }
}