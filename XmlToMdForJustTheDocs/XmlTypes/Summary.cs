using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace XmlToMdForJustTheDocs.XmlTypes
{

    [XmlRoot(ElementName = "summary")]
    public class Summary : IXmlSerializable
    {
        #region Public Fields
        private string _Text = string.Empty;
        public string Text { get => _Text; set => _Text = value; }

        private List<string> _Remarks = new List<string>();
        public List<string> Remarks { get => _Remarks; set => Remarks = value; }

        private List<Member> _See = new List<Member>();
        public List<Member> See { get => _See; set => _See = value; }
        #endregion

        #region Public Methods
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
#pragma warning disable CS8602 // null checking implemented.
                            member.Name = element.Attribute("cref").Value == null ? 
                                string.Empty : element.Attribute("cref").Value;
#pragma warning restore CS8602 // null checking implemented.
                            sb.Append($"{{{See.Count}}} ");
                            See.Add(member);
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
#pragma warning disable CS8602 // null checking implemented.
                    var result = (node as XText).Value == null ? string.Empty : (node as XText).Value;
#pragma warning restore CS8602 // null checking implemented.
                    sb.Append(
                        result.Replace("\n", " ").Trim() + " "
                    );
                }
            }
        }
        #endregion
    }
}