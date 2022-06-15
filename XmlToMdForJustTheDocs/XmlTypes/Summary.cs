using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace XmlToMdForJustTheDocs.XmlTypes;

[XmlRoot(ElementName = "summary")]
public class Summary : IXmlSerializable
{
    public string Text { get; set; }

    public List<string> Remarks { get; set; } = new();

    public List<Member> See { get; set; } = new();

    public XmlSchema? GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        var sb = new StringBuilder();
        var summary = XElement.Parse(reader.ReadOuterXml());
        ParseSummaryRecursive(summary, sb);
        Text = sb.ToString().Trim();
    }

    public void WriteXml(XmlWriter writer)
    {
        throw new NotImplementedException();
    }

    private void ParseSummaryRecursive(XElement root, StringBuilder sb)
    {
        foreach (var node in root.Nodes())
        {
            if (node is XElement element)
            {
                switch (element.Name.LocalName)
                {
                    case "see":
                        var member = new Member();
                        member.Name = element.Attribute("cref").Value;
                        sb.Append(member.MemberName + " ");
                        break;
                    case "remarks":
                        Remarks.Add(element.Value);
                        break;
                    case "para":
                        sb.AppendLine();
                        ParseSummaryRecursive(element, sb);
                        break;
                }
            }
            else
            {
                sb.Append((node as XText).Value.Trim() + " ");
            }
        }
    }
}