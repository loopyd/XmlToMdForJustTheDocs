namespace XmlToMdForJustTheDocs
{
    static class FileUtilities
    {
        #region Public Methods
        public static bool IsDirectory(this FileSystemInfo path)
        {
            return path is DirectoryInfo;
        }

        public static bool IsFile(this FileSystemInfo path)
        {
            return path is not DirectoryInfo;
        }

        public static FileSystemInfo CreateChildFolder(this FileSystemInfo root, string folder)
        {
            if (!root.IsDirectory())
            {
                throw new ArgumentException("provided FileSystemInfo was not a directory but a file");
            }

            return Directory.CreateDirectory(root.FullName + "/" + folder);
        }
        #endregion
    }
}