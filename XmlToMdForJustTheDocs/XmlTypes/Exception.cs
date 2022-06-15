using System.Xml.Serialization;

namespace XmlToMdForJustTheDocs.XmlTypes;

[XmlRoot(ElementName="exception")]
public class Exception { 

    [XmlAttribute(AttributeName="cref")] 
    public string Cref { get; set; } 

    [XmlText] 
    public string Text { get; set; } 
}