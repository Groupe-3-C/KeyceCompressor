using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using KeyceCompressor.Utils;

namespace KeyceCompressor.Zip
{
    public static class ZipCompressor
    {
        public static void Compress(string source, string output, IProgress<int> progress)
        {
            if (File.Exists(output))
                File.Delete(output);

            var files = new List<(string Rel, string Full, bool Dir)>();
            string baseDir = Directory.Exists(source) ? source : Path.GetDirectoryName(source) ?? Environment.CurrentDirectory;

            if (Directory.Exists(source))
                Collect(source, baseDir, files);
            else
                files.Add((Path.GetFileName(source), source, false));

            using (var zipArchive = ZipFile.Open(output, ZipArchiveMode.Create))
            {
                int done = 0;
                int total = files.Count;

                foreach (var file in files) // Changé de "var f" → "var file" pour éviter le warning de déconstruction
                {
                    if (file.Dir)
                        zipArchive.CreateEntry(file.Rel);
                    else
                        zipArchive.CreateEntryFromFile(file.Full, file.Rel);

                    done++;
                    progress.Report(total > 0 ? (done * 100 / total) : 100);
                }
            }
        }

        public static void Decompress(string zipPath, string outputFolder, IProgress<int> progress)
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            using (var archive = ZipFile.OpenRead(zipPath))
            {
                int total = archive.Entries.Count;
                int done = 0;

                foreach (var entry in archive.Entries)
                {
                    string fullPath = Path.Combine(outputFolder, entry.FullName.Replace('/', Path.DirectorySeparatorChar));

                    if (entry.FullName.EndsWith("/"))
                    {
                        Directory.CreateDirectory(fullPath);
                    }
                    else
                    {
                        string dir = Path.GetDirectoryName(fullPath);
                        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                            Directory.CreateDirectory(dir);

                        entry.ExtractToFile(fullPath, true);
                    }

                    done++;
                    progress.Report(total > 0 ? (done * 100 / total) : 100);
                }
            }
        }

        private static void Collect(string dir, string baseDir, List<(string Rel, string Full, bool Dir)> list)
        {
            foreach (var d in Directory.GetDirectories(dir))
            {
                string rel = PathUtils.GetRelativePath(baseDir, d).Replace(Path.DirectorySeparatorChar, '/') + "/";
                list.Add((rel, d, true));
                Collect(d, baseDir, list);
            }

            foreach (var f in Directory.GetFiles(dir))
            {
                string rel = PathUtils.GetRelativePath(baseDir, f).Replace(Path.DirectorySeparatorChar, '/');
                list.Add((rel, f, false));
            }
        }
    }
}