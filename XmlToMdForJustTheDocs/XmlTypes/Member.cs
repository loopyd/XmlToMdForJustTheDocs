using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace XmlToMdForJustTheDocs.XmlTypes;

[XmlRoot(ElementName = "member")]
public class Member
{
    public enum MemberEnum
    {
        Field,
        Property,
        Type,
        Event,
        Method
    }
    
    /// <summary>
    ///     Dictionary of types that XML can have for their members. Expands each element into it's fully qualified name
    /// </summary>
    private static readonly Dictionary<string, MemberEnum> _MemberNamePrefixDict = new(StringComparer.OrdinalIgnoreCase)
    {
        ["F"] = MemberEnum.Field,
        ["P"] = MemberEnum.Property,
        ["T"] = MemberEnum.Type,
        ["E"] = MemberEnum.Event,
        ["M"] = MemberEnum.Method
    };

    private bool _isCreated = false;
    private string _ns = null!;
    private string _memberName = null!;
    private string _className = null!;
    private MemberEnum _typeName;

    /// <summary>
    ///     Member full summary name
    /// </summary>
    [XmlElement(ElementName = "summary")]
    public Summary? Summary { get; set; }

    /// <summary>
    ///     Fully qualified name from XML in format X:Namespace.Class...
    /// </summary>
    [XmlAttribute(AttributeName = "name")]
    public string Name { get; set; }

    /// <summary>
    ///     Body of the member
    /// </summary>
    [XmlText]
    public string Text { get; set; }

    /// <summary>
    ///     Should this member inherit documentation from it's predecesor
    /// </summary>
    [XmlElement(ElementName = "inheritdoc")]
    public object Inheritdoc { get; set; }

    /// <summary>
    ///     Parameters usually used for methods.
    /// </summary>
    [XmlElement(ElementName = "param")]
    public List<Param> Param { get; set; }

    /// <summary>
    ///     What give (ussual method) returns
    /// </summary>
    [XmlElement(ElementName = "returns")]
    public object Returns { get; set; }

    /// <summary>
    ///     Remarks for the member
    /// </summary>
    [XmlElement(ElementName = "remarks")]
    public string Remarks { get; set; }

    /// <summary>
    ///     Exception that given member throws
    /// </summary>
    [XmlElement(ElementName = "exception")]
    public Exception Exception { get; set; }

    /// <summary>
    ///     Generic type parameters
    /// </summary>
    [XmlElement(ElementName = "typeparam")]
    public Typeparam Typeparam { get; set; }

    /// <summary>
    ///     Helper property to determine if given type is a method
    /// </summary>
    [XmlIgnore]
    public bool IsMethod => Name.StartsWith("M");

    /// <summary>
    ///     Helper property to determine if given type is a field
    /// </summary>
    [XmlIgnore]
    public bool IsField => Name.StartsWith("F");

    /// <summary>
    ///     Helper property to determine if given type is a property
    /// </summary>
    [XmlIgnore]
    public bool IsProperty => Name.StartsWith("P");

    /// <summary>
    ///     Helper property to determine if given type is a type
    /// </summary>
    [XmlIgnore]
    public bool IsType => Name.StartsWith("T");

    /// <summary>
    ///     Helper property to determine if given type is a event
    /// </summary>
    [XmlIgnore]
    public bool IsEvent => Name.StartsWith("E");

    /// <summary>
    ///     Converted value from fully qualified XML <see cref="Name"/> to a namespace.
    /// </summary>
    [XmlIgnore]
    public string Namespace
    {
        get
        {
            if(!_isCreated)             
                ExtractNameAndBodyFromMember();
            return _ns;
        }
        private set => _ns = value;
    }

    /// <summary>
    ///     Converted value from full qualified XML <see cref="Name"/> to a member name.
    /// </summary>
    [XmlIgnore]
    public string MemberName
    {
        get
        {
            if(!_isCreated)             
                ExtractNameAndBodyFromMember();
            return _memberName;
        }
        private set => _memberName = value;
    }

    /// <summary>
    ///     Converted value from full qualified XML <see cref="Name"/> to a class name.
    /// </summary>
    [XmlIgnore]
    public string ClassName
    {
        get
        {
            if(!_isCreated)             
                ExtractNameAndBodyFromMember();
            return _className;
        }
        private set => _className = value;
    }

    /// <summary>
    ///     Converted value from full qualified XML <see cref="Name"/> to a type name.
    /// </summary>
    [XmlIgnore]
    public MemberEnum TypeName
    {
        get
        {
            if(!_isCreated)             
                ExtractNameAndBodyFromMember();
            return _typeName;
        }
        private set => _typeName = value;
    }

    public void InitializeMember()
    {
        ExtractNameAndBodyFromMember();
    }

    private void ExtractNameAndBodyFromMember()
    {
        if(_isCreated) return;
        _isCreated = true;
        if (string.IsNullOrEmpty(Name))
        {
            return;
        }

        TypeName = GetTypeName(Name);
        var nameSplit = Regex.Split(Name, @"([\(|\`].+[\)])");
        if (nameSplit.Length <= 1)
        {
            ExtractNamespaceAndNames(Name);
        }
        else
        {
            ExtractNamespaceAndNames(nameSplit[0]);
            var parameters = Regex.Match(nameSplit[1], @"\((.+?[,)])+");
            var list = new List<string>();
            for (var groupCtr = 1; groupCtr < parameters.Groups.Count; groupCtr++)
            {
                for (var captureCtr = 0; captureCtr < parameters.Groups[groupCtr].Captures.Count; captureCtr++)
                {
                    list.Add(parameters.Groups[groupCtr].Captures[captureCtr].Value.TrimEnd(',', ')'));
                }
            }

            var genericParametersCount = Regex.Match(nameSplit[1], @"``(\w+)\(");
            var isGeneric = genericParametersCount.Length != 0;

            for (var i = 0; i < list.Count; i++)
            {
                if (list[i][^1] == '@')
                {
                    Param[i].ByRef = true;
                }

                Param[i].Type = list[i].TrimEnd('@');
            }
        }
        /*int IndexOfOrMax(string text, char sign)
        {
            var index = text.IndexOf(sign);
            return index == -1
                ? int.MaxValue
                : index;
        }*/
        
        /*var newName = Name.Split(':', 2)[1];
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

        TypeName = typeName;
        Name = name.Trim();
        Namespace = namespaceName.Trim();*/
    }

    /// <summary>
    ///     Extracts <see cref="Namespace"/> <see cref="MemberName"/> and <see cref="ClassName"/> to this instance members based on XML name without it's parameters
    /// </summary>
    /// <param name="name"></param>
    /// <example>
    ///     <code>
    ///         ExtractNamespaceAndNames("T:System.ArgumentException"); // Correct
    ///         ExtractNamespaceAndNames("P:ProgLibrary.PrecomputedCulling.MonoBehaviours.IdCubemapCamera.Settings"); // Correct
    ///         ExtractNamespaceAndNames("M:ProgLibrary.PrecomputedCulling.Utils.ArrayUtils.CopyArray``1(``0[])"); // Incorrect
    ///     </code>
    /// </example>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private void ExtractNamespaceAndNames(string name)
    {
        var fullName = Regex.Match(name, @"\w:(.+)").Groups[1].Value;
        var split = fullName.Split(".");
        switch (TypeName)
        {
            case MemberEnum.Type:
                Namespace = string.Join(".", split, 0, split.Length - 1);
                MemberName = split[^1];
                ClassName = split[^1];
                break;
            case MemberEnum.Field:
            case MemberEnum.Property:
            case MemberEnum.Event:
            case MemberEnum.Method:
                Namespace = string.Join(".", split, 0, split.Length - 2);
                MemberName = split[^1];
                ClassName = split[^2];
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    ///     Returns TypeName based on <see cref="_MemberNamePrefixDict"/>
    /// </summary>
    /// <param name="name">Requires fully qualified xml name</param>
    /// <returns>Returns new string based on <see cref="_MemberNamePrefixDict"/></returns>
    private static MemberEnum GetTypeName(string name)
    {
        var typeName = _MemberNamePrefixDict[name[0].ToString()];
        return typeName;
    }
}