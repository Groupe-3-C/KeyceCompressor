using System;
using System.IO;
using System.Text;

namespace KeyceCompressor.Huffman
{
    public static class HuffmanDecompressor
    {
        public static void Decompress(string keycePath, string outputFolder, IProgress<int> progress)
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            using (var fs = new FileStream(keycePath, FileMode.Open, FileAccess.Read))
            using (var br = new BinaryReader(fs, Encoding.UTF8))
            {
                // Vérification du magic number
                if (Encoding.ASCII.GetString(br.ReadBytes(4)) != "KEYC")
                    throw new InvalidDataException("Ce n'est pas un fichier .keyce valide.");

                int fileCount = br.ReadInt32();

                for (int i = 0; i < fileCount; i++)
                {
                    int pathLength = br.ReadInt32();
                    string relativePath = Encoding.UTF8.GetString(br.ReadBytes(pathLength));
                    bool isDirectory = br.ReadBoolean();
                    string fullPath = Path.Combine(outputFolder, relativePath);

                    long originalSize = br.ReadInt64();
                    long compressedSize = br.ReadInt64();

                    if (isDirectory)
                    {
                        Directory.CreateDirectory(fullPath);
                    }
                    else
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? outputFolder);

                        if (originalSize == 0)
                        {
                            File.Create(fullPath).Close();
                            continue;
                        }

                        // Enregistrement de la position de début de l'arbre pour le calcul de la fin du bloc
                        long startOfTree = fs.Position;
                        var root = HuffmanTree.Deserialize(br);
                        long endOfTree = fs.Position;

                        var bitReader = new BitReader(fs);

                        using (var outputStream = new FileStream(fullPath, FileMode.Create))
                        {
                            var node = root;
                            long bytesWritten = 0;

                            // Décompression bit par bit jusqu'à atteindre la taille originale
                            while (bytesWritten < originalSize)
                            {
                                bool bit = bitReader.ReadBit();
                                node = bit ? node.Right : node.Left;

                                if (node.IsLeaf)
                                {
                                    outputStream.WriteByte(node.ByteValue.Value);
                                    bytesWritten++;
                                    node = root;
                                }
                            }

                            // Nous devons nous assurer que nous avons lu exactement originalSize octets.
                            if (bytesWritten != originalSize)
                            {
                                throw new InvalidDataException("Erreur de décompression : la taille décompressée ne correspond pas à la taille originale.");
                            }
                        }

                        // S'assurer que nous sommes à la bonne position pour le prochain fichier
                        // La position finale attendue est : startOfTree + compressedSize.
                        long expectedEndPosition = startOfTree + compressedSize;

                        // S'assurer que nous sommes à la bonne position pour le prochain fichier
                        if (fs.Position < expectedEndPosition)
                            fs.Seek(expectedEndPosition - fs.Position, SeekOrigin.Current);

                    }

                    progress.Report((i + 1) * 100 / fileCount);
                }
            }
        }
    }
}
