using System.Text.RegularExpressions;

namespace XmlToMdForJustTheDocs.Utils
{
    public static class TextUtilities
    {
        #region Private Fields
        private static Regex _removeSpacesRegex;
        #endregion

        #region Methods
        static TextUtilities()
        {
            RegexOptions options = RegexOptions.Compiled;
            _removeSpacesRegex = new Regex("[ ]{2,}", options);
        }

        public static string RemovePadding(this string text)
        {
            return _removeSpacesRegex.Replace(text, " ");
        }
        #endregion
    }
}