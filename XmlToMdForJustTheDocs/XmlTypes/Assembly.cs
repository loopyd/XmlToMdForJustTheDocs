using System.Xml.Serialization;

namespace XmlToMdForJustTheDocs.XmlTypes;

[XmlRoot(ElementName="assembly")]
public class Assembly { 

    [XmlElement(ElementName="name")] 
    public string Name { get; set; } 
}