namespace XmlToMdForJustTheDocs.Models
{
    public class Options
    {
        #region Public Fields
        private string _InputFile = string.Empty;
        public string InputFile { get => _InputFile; set => _InputFile = value; }
        private string _OutputDirectory = string.Empty;
        public string OutputDirectory { get => _OutputDirectory; set => _OutputDirectory = value; }
        private string _Version = string.Empty;
        public string Version { get => _Version; set => _Version = value; }
        #endregion
    }
}