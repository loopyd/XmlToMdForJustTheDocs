using System.Xml.Serialization;

namespace XmlToMdForJustTheDocs.XmlTypes;

[XmlRoot(ElementName="see")]
public class See { 

    [XmlAttribute(AttributeName="cref")] 
    public string Cref { get; set; } 
}