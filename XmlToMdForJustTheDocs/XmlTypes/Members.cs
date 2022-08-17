using System.Xml.Serialization;

namespace XmlToMdForJustTheDocs.XmlTypes
{
    [XmlRoot(ElementName = "members")]
    public class Members
    {

        #region Public Fields
        private List<Member> _MembersList = new List<Member>();
        [XmlElement(ElementName = "member")]
        public List<Member> MembersList { get => _MembersList; set => _MembersList = value; }

        private Lazy<List<string>> _Namespaces = new Lazy<List<string>>();
        [XmlIgnore]
        public Lazy<List<string>> Namespaces { get => _Namespaces; set => _Namespaces = value; }

        private Lazy<List<Member>> _Types = new Lazy<List<Member>>();
        [XmlIgnore]
        public Lazy<List<Member>> Types { get => _Types; set => _Types = value; }

        private Lazy<Dictionary<string, TypeMembers>> _TypeMembers = new Lazy<Dictionary<string, TypeMembers>>();
        [XmlIgnore]
        public Lazy<Dictionary<string, TypeMembers>> TypeMembers { get => _TypeMembers; set => _TypeMembers = value; }
        #endregion

        #region Constructor
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
        #endregion

        #region Methods
        public Member ResolveToMember(string type)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    public class TypeMembers
    {
        #region Public Fields
        private Member _Type = new Member();
        public Member Type { get => _Type; set => _Type = value; }

        private List<Member> _Fields = new List<Member>();
        public List<Member> Fields { get => _Fields; set => _Fields = value; }

        private List<Member> _Events = new List<Member>();
        public List<Member> Events { get => _Events; set => Events = value; }

        private List<Member> _Constructors = new List<Member>();
        public List<Member> Constructors { get => _Constructors; set => _Constructors = value; }

        private List<Member> _Properties = new List<Member>();
        public List<Member> Properties { get => _Properties; set => _Properties = value; }

        private List<Member> _Methods = new List<Member>();
        public List<Member> Methods { get => _Methods; set => _Methods = value; }
        #endregion
    }
}