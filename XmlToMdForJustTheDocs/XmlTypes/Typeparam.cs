using System.Xml.Serialization;

namespace XmlToMdForJustTheDocs.XmlTypes
{
    [XmlRoot(ElementName = "typeparam")]
    public class Typeparam
    {

        #region Public Fields
        private string _Name = string.Empty;
        [XmlAttribute(AttributeName = "name")]
        public string Name { get => _Name; set => _Name = value; }

        private string _Text = string.Empty;
        [XmlText]
        public string Text { get => _Text; set => _Text = value; }
        #endregion
    }
}