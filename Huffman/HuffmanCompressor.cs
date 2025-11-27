using KeyceCompressor.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq; // Ajouté pour la méthode Select

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

            using (var fs = new FileStream(output, FileMode.Create))
            using (var bw = new BinaryWriter(fs, Encoding.UTF8))
            {
                // Magic number
                bw.Write(Encoding.ASCII.GetBytes("KEYC"));
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
                        bw.Write(0L); // Taille originale
                        bw.Write(0L); // Taille compressée
                    }
                    else
                    {
                        var data = File.ReadAllBytes(f.Full);
                        bw.Write((long)data.Length); // Taille originale

                        if (data.Length == 0)
                        {
                            bw.Write(0L); // Taille compressée
                            continue;
                        }

                        // 1. Calcul des fréquences
                        var freq = new Dictionary<byte, long>();
                        foreach (var b in data)
                        {
                            if (freq.ContainsKey(b)) freq[b]++;
                            else freq[b] = 1;
                        }

                        // 2. Construction de l'arbre et des codes
                        var tree = new HuffmanTree(freq);
                        var codes = tree.GetCodes();

                        // Placeholder pour la taille compressée
                        long compressedDataStart = fs.Position;
                        bw.Write(0L);

                        // Sérialisation de l'arbre
                        tree.Serialize(bw);

                        // 3. Compression du contenu
                        var bits = new List<bool>();
                        foreach (var b in data)
                        {
                            // L'extension System.Linq est nécessaire pour .Select
                            bits.AddRange(codes[b].Select(c => c == '1'));
                        }

                        // Padding
                        int pad = (8 - bits.Count % 8) % 8;
                        for (int p = 0; p < pad; p++) bits.Add(false);

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

                        // Mise à jour de la taille compressée
                        long compressedDataEnd = fs.Position;
                        // La taille compressée est la taille totale du bloc (arbre + bits) moins la taille du placeholder (8 octets)
                        long compressedSize = compressedDataEnd - (compressedDataStart + 8);

                        // Sauvegarder la position actuelle
                        long currentPosition = fs.Position;

                        // Écrire la taille compressée
                        fs.Seek(compressedDataStart, SeekOrigin.Begin);
                        bw.Write(compressedSize);

                        // Retourner à la position après les données compressées
                        fs.Seek(currentPosition, SeekOrigin.Begin);
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
