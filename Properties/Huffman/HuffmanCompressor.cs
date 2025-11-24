using KeyceCompressor.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace KeyceCompressor.Huffman
{
    public static class HuffmanCompressor
    {
        public static void Compress(string source, string output, IProgress<int> progress)
        {
            var files = new List<(string Rel, string Full, bool Dir)>();
            string baseDir = Directory.Exists(source) ? source : Path.GetDirectoryName(source) ?? Environment.CurrentDirectory;

            if (Directory.Exists(source))
                Collect(source, baseDir, files);
            else
                files.Add((Path.GetFileName(source), source, false));

            // 1. Calcul des fréquences (sur tous les fichiers)
            var freq = new Dictionary<byte, long>();
            foreach (var f in files)
            {
                if (f.Dir) continue;
                var data = File.ReadAllBytes(f.Full);
                foreach (var b in data)
                {
                    byte key = b;
                    long cur;
                    if (freq.TryGetValue(key, out cur)) freq[key] = cur + 1;
                    else freq[key] = 1;
                }
            }

            // 2. Construction de l'arbre et des codes
            var tree = new HuffmanTree(freq);
            var codes = tree.GetCodes();

            // 3. Écriture du fichier .keyce
            using (var fs = new FileStream(output, FileMode.Create))
            using (var bw = new BinaryWriter(fs, Encoding.UTF8))
            {
                // Magic number
                bw.Write(Encoding.ASCII.GetBytes("KEYC"));

                // Placeholder pour la taille de l'arbre
                bw.Write(0L);

                // Sérialisation de l'arbre
                tree.Serialize(bw);
                long treeEnd = fs.Position;

                // Écriture de la taille de l'arbre
                fs.Seek(4, SeekOrigin.Begin);
                bw.Write(treeEnd - 12); // 12 = 4 (magic) + 8 (long)

                // Retour à la fin de l'arbre
                fs.Seek(treeEnd, SeekOrigin.Begin);

                // Nombre de fichiers/dossiers
                bw.Write(files.Count);

                int done = 0;
                foreach (var f in files)
                {
                    var relBytes = Encoding.UTF8.GetBytes(f.Rel);
                    bw.Write(relBytes.Length);
                    bw.Write(relBytes);
                    bw.Write(f.Dir);

                    if (f.Dir)
                    {
                        // Pour les dossiers, on écrit une taille de 0
                        bw.Write(0L);
                    }
                    else
                    {
                        // Compression du contenu
                        var data = File.ReadAllBytes(f.Full);
                        var bits = new List<bool>();

                        foreach (var b in data)
                        {
                            string code = codes[b];
                            foreach (char bitChar in code)
                            {
                                bits.Add(bitChar == '1');
                            }
                        }

                        // Padding
                        int pad = (8 - bits.Count % 8) % 8;
                        for (int p = 0; p < pad; p++) bits.Add(false);

                        // Écriture de la taille en octets
                        bw.Write((long)(bits.Count / 8));

                        // Écriture des octets compressés
                        for (int i = 0; i < bits.Count; i += 8)
                        {
                            byte currentByte = 0;
                            for (int j = 0; j < 8; j++)
                            {
                                if (bits[i + j])
                                {
                                    currentByte |= (byte)(1 << (7 - j));
                                }
                            }
                            bw.Write(currentByte);
                        }
                    }
                    progress.Report(++done * 100 / files.Count);
                }
            }
        }

        private static void Collect(string dir, string baseDir, List<(string, string, bool)> list)
        {
            foreach (var d in Directory.GetDirectories(dir))
            {
                var rel = PathUtils.GetRelativePath(baseDir, d).Replace(Path.DirectorySeparatorChar, '/');
                list.Add((rel + "/", d, true));
                Collect(d, baseDir, list);
            }
            foreach (var f in Directory.GetFiles(dir))
            {
                var rel = PathUtils.GetRelativePath(baseDir, f).Replace(Path.DirectorySeparatorChar, '/');
                list.Add((rel, f, false));
            }
        }
    }
}
