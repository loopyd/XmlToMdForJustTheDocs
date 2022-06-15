using System.Text.RegularExpressions;

namespace XmlToMdForJustTheDocs;

public static class TextUtils
{
    private static Regex _removeSpacesRegex;
    
    static TextUtils()
    {
        RegexOptions options = RegexOptions.Compiled;
        _removeSpacesRegex = new Regex("[ ]{2,}", options);  
    }

    public static string RemovePadding(this string text)
    {
        return _removeSpacesRegex.Replace(text, " ");
    }
}