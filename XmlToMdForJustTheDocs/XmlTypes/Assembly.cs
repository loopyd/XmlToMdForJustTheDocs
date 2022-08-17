using System.Xml.Serialization;

namespace XmlToMdForJustTheDocs.XmlTypes
{

    [XmlRoot(ElementName = "assembly")]
    public class Assembly
    {
        #region Public Fields
        private string _Name = string.Empty;
        [XmlElement(ElementName = "name")]
        public string Name { get => _Name; set => _Name = value; }
        #endregion
    }
}