using System.Xml.Serialization;

namespace XmlToMdForJustTheDocs.XmlTypes
{
    [XmlRoot(ElementName = "exception")]
    public class Exception
    {

        #region Public Fields
        private string _Cref = string.Empty;
        [XmlAttribute(AttributeName = "cref")]
        public string Cref { get => _Cref; set => _Cref = value; }

        private string _Text = string.Empty;
        [XmlText]
        public string Text { get => _Text; set => _Text = value; }
        #endregion
    }
}