using System;
using System.IO;


namespace KeyceCompressor.Utils
{
    public static class PathUtils
    {
        public static string MakeValidFileName(string name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name;
        }

        public static string GetRelativePath(string basePath, string fullPath)
        {
            if (string.IsNullOrEmpty(basePath)) return fullPath;
            var baseFull = Path.GetFullPath(basePath);
            var fullFull = Path.GetFullPath(fullPath);

            if (!baseFull.EndsWith(Path.DirectorySeparatorChar.ToString()))
                baseFull += Path.DirectorySeparatorChar;

            var baseUri = new Uri(baseFull);
            var fullUri = new Uri(fullFull);
            var relative = baseUri.MakeRelativeUri(fullUri).ToString();
            relative = Uri.UnescapeDataString(relative);
            // Remplace les séparateurs d'URI par les séparateurs de système
            return relative.Replace('/', Path.DirectorySeparatorChar);
        }

        public static string GetSafePath(string path)
        {
            // Retourne le chemin fourni ou le bureau par défaut
            return string.IsNullOrWhiteSpace(path) ? Environment.GetFolderPath(Environment.SpecialFolder.Desktop) : path;
        }
    }
}
