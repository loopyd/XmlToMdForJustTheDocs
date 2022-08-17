using System.Xml.Serialization;

namespace XmlToMdForJustTheDocs.XmlTypes
{
    [XmlRoot(ElementName = "doc")]
    public class Documentation
    {
        #region Public Fields
        private Assembly _Assembly = new Assembly();
        [XmlElement(ElementName = "assembly")]
        public Assembly Assembly { get => _Assembly; set => _Assembly = value; }

        private Members _Members = new Members();
        [XmlElement(ElementName = "members")]
        public Members Members { get => _Members; set => _Members = value; }
        #endregion
    }
}