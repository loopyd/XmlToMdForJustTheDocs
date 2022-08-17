namespace XmlToMdForJustTheDocs.Models
{
    public class Jtd
    {
        #region Public Fields
        private string _Layout = "default";
        public string Layout { get => _Layout; set => _Layout = value; }

        private string _Title = string.Empty;
        public string Title { get => _Title; set => _Title = value; }

        private int _NavigationOrder = 0;
        public int NavigationOrder { get => _NavigationOrder; set => _NavigationOrder = value; }

        private string _Permalink = string.Empty;
        public string Permalink { get => _Permalink; set => _Permalink = value; }

        private bool _HasChildren = false;
        public bool HasChildren { get => _HasChildren; set => _HasChildren = value; }
        #endregion
    }
}