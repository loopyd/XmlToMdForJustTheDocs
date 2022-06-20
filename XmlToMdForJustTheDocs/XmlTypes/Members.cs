using System.Xml.Serialization;

namespace XmlToMdForJustTheDocs.XmlTypes;

[XmlRoot(ElementName="members")]
public class Members { 

    [XmlElement(ElementName="member")] 
    public List<Member> MembersList { get; set; }
    
    [XmlIgnore]
    public Lazy<List<string>> Namespaces { get; set; }
    [XmlIgnore]
    public Lazy<List<Member>> Types { get; set; }
    [XmlIgnore]
    public Lazy<Dictionary<string, TypeMembers>> TypeMembers { get; set; }

    public Members()
    { 
        Types = new Lazy<List<Member>>(() =>
        {
            return MembersList!.Where(member => member.IsType).ToList();
        });
        Namespaces = new Lazy<List<string>>(() =>
        {
            return MembersList!.Select(member => member.Namespace).ToList();
        });
        TypeMembers = new Lazy<Dictionary<string, TypeMembers>>(() =>
        {
            Dictionary<string, TypeMembers> typesMap = new();
            foreach (var member in MembersList)
            {
                if (!typesMap.ContainsKey(member.ClassName))
                {
                    typesMap[member.ClassName] = new TypeMembers();
                }
                var item = typesMap[member.ClassName];
                
                if (member.IsType)
                {
                    item.Type = member;
                }
                if (member.IsField)
                {
                    item.Fields.Add(member);
                }
                if (member.IsProperty)
                {
                    item.Properties.Add(member);
                }
                if (member.IsEvent)
                {
                    item.Events.Add(member);
                }
                if (member.IsMethod)
                {
                    if (member.Name == "#ctor")
                    {
                        item.Constructors.Add(member);
                    }
                    else
                    {
                        item.Methods.Add(member);
                    }
                }
            }

            return typesMap;
        });
    }

    public Member ResolveToMember(string type)
    {
        throw new NotImplementedException();
    }
}

public class TypeMembers
{
    public Member? Type { get; set; } = null;
    public List<Member> Fields { get; set; } = new ();
    public List<Member> Events { get; set; } = new ();
    public List<Member> Constructors { get; set; } = new ();
    public List<Member> Properties { get; set; } = new ();
    public List<Member> Methods { get; set; } = new ();
}