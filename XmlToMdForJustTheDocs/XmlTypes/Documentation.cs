using System.Xml.Serialization;

namespace XmlToMdForJustTheDocs.XmlTypes;

[XmlRoot(ElementName="doc")]
public class Documentation { 

    [XmlElement(ElementName="assembly")] 
    public Assembly Assembly { get; set; } = null!;

    [XmlElement(ElementName="members")] 
    public Members Members { get; set; } = null!;

}