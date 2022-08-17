namespace XmlToMdForJustTheDocs.Utils
{
    // This isn't used yet but probably should be in some places.
    public static class TypeUtilities
    {
        public static bool TryCast<T>(this object obj, out T result)
        {
            if (obj is T)
            {
                result = (T)obj;
                return true;
            }

            result = default(T)!;
            return false;
        }
    }
}