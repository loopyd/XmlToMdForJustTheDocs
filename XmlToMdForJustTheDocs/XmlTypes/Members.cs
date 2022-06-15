using System.Xml.Serialization;

namespace XmlToMdForJustTheDocs.XmlTypes;

[XmlRoot(ElementName="members")]
public class Members { 

    [XmlElement(ElementName="member")] 
    public List<Member> MembersList { get; set; } 
}