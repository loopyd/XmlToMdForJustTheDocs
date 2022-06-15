using System.Xml.Serialization;

namespace XmlToMdForJustTheDocs.XmlTypes;

[XmlRoot(ElementName = "member")]
public class Member
{
    private static readonly Dictionary<string, string> _MemberNamePrefixDict = new(StringComparer.OrdinalIgnoreCase)
    {
        ["F:"] = "Field",
        ["P:"] = "Property",
        ["T:"] = "Type",
        ["E:"] = "Event",
        ["M:"] = "Method"
    };

    private string _name;

    [XmlElement(ElementName = "summary")]
    public Summary? Summary { get; set; }

    [XmlAttribute(AttributeName = "name")]
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            var extract = ExtractNameAndBodyFromMember();
            Namespace = extract.Namespace;
            MemberName = extract.MemberName;
            TypeName = extract.TypeName;
        }
    }

    [XmlText]
    public string Text { get; set; }

    [XmlElement(ElementName = "inheritdoc")]
    public object Inheritdoc { get; set; }

    [XmlElement(ElementName = "param")]
    public List<Param> Param { get; set; }

    [XmlElement(ElementName = "returns")]
    public object Returns { get; set; }

    [XmlElement(ElementName = "remarks")]
    public string Remarks { get; set; }

    [XmlElement(ElementName = "exception")]
    public Exception Exception { get; set; }

    [XmlElement(ElementName = "typeparam")]
    public Typeparam Typeparam { get; set; }

    [XmlIgnore]
    public bool IsMethod => Name.StartsWith("M");

    [XmlIgnore]
    public bool IsField => Name.StartsWith("F");

    [XmlIgnore]
    public bool IsProperty => Name.StartsWith("P");

    [XmlIgnore]
    public bool IsType => Name.StartsWith("T");

    [XmlIgnore]
    public bool IsEvent => Name.StartsWith("E");

    [XmlIgnore]
    public string Namespace { get; private set; } = null!;

    [XmlIgnore]
    public string MemberName { get; private set; } = null!;

    [XmlIgnore]
    public string TypeName { get; private set; } = null!;

    private (string TypeName, string MemberName, string Namespace) ExtractNameAndBodyFromMember()
    {
        int IndexOfOrMax(string text, char sign)
        {
            var index = text.IndexOf(sign);
            return index == -1
                ? int.MaxValue
                : index;
        }

        if (string.IsNullOrEmpty(Name))
        {
            return (String.Empty, String.Empty, String.Empty);
        }

        var newName = Name;
        var typeName = _MemberNamePrefixDict[newName.Split(':', 2)[0] + ":"];
        newName = newName.Split(':', 2)[1];
        /*newName = _PrefixReplacerRegex.Replace(newName,
            match => _MemberNamePrefixDict[match.Value] + " "); //expand prefixes into more verbose words for member.*/
        var endOfMethodName = Math.Min(IndexOfOrMax(newName, '`'), IndexOfOrMax(newName, '('));
        string name, namespaceName;
        if (endOfMethodName != int.MaxValue)
        {
            var nsWithName = newName.Substring(0, endOfMethodName);
            var indexOfLastDot = nsWithName.LastIndexOf('.');
            namespaceName = nsWithName.Substring(0, indexOfLastDot);
            name = nsWithName.Substring(indexOfLastDot + 1, nsWithName.Length - indexOfLastDot - 1);
        }
        else
        {
            var indexOfLastDot = newName.LastIndexOf('.');
            if (indexOfLastDot == -1)
            {
                namespaceName = "::global";
                name = newName;
            }
            else
            {
                namespaceName = newName.Substring(0, indexOfLastDot);
                name = newName.Substring(indexOfLastDot + 1, newName.Length - indexOfLastDot - 1);
            }
        }

        return (typeName.Trim(), name.Trim(), namespaceName.Trim());
    }
}