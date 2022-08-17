using System.Xml.Serialization;

namespace XmlToMdForJustTheDocs.XmlTypes
{

    [XmlRoot(ElementName = "param")]
    public class Param
    {
        #region Public Fields
        private string _Name = string.Empty;
        [XmlAttribute(AttributeName = "name")]
        public string Name { get => _Name; set => _Name = value; }

        private string _Text = string.Empty;
        [XmlText]
        public string Text { get => _Text; set => _Text = value; }

        private See _See = new See();
        [XmlElement(ElementName = "see")]
        public See See { get => _See; set => _See = value; }

        private bool _ByRef = false;
        [XmlIgnore]
        public bool ByRef { get => _ByRef; set => _ByRef = value; }

        private string _Type = string.Empty;
        [XmlIgnore]
        public string Type { get => _Type; set => _Type = value; }
        #endregion
    }
}