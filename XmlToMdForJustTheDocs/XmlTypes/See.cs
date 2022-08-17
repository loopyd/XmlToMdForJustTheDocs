using System.Xml.Serialization;

namespace XmlToMdForJustTheDocs.XmlTypes
{

    [XmlRoot(ElementName = "see")]
    public class See
    {
        #region Public Fields
        private string _Cref = string.Empty;
        [XmlAttribute(AttributeName = "cref")]
        public string Cref { get => _Cref; set => _Cref = value; }
        #endregion
    }
}