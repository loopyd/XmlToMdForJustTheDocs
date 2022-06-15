using System.Xml.Serialization;

namespace XmlToMdForJustTheDocs.XmlTypes;

[XmlRoot(ElementName="param")]
public class Param { 

    [XmlAttribute(AttributeName="name")] 
    public string Name { get; set; } 

    [XmlText] 
    public string Text { get; set; } 

    [XmlElement(ElementName="see")] 
    public See See { get; set; } 
}