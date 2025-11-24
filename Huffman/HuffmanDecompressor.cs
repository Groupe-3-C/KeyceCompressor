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

                long treeSize = br.ReadInt64();
                // Deserialize returns the root HuffmanNode
                var root = HuffmanTree.Deserialize(br);

                int fileCount = br.ReadInt32();
                // Le BitReader doit commencer à lire après les métadonnées
                var bitReader = new BitReader(fs);

                for (int i = 0; i < fileCount; i++)
                {
                    int pathLength = br.ReadInt32();
                    string relativePath = Encoding.UTF8.GetString(br.ReadBytes(pathLength));
                    bool isDirectory = br.ReadBoolean();
                    string fullPath = Path.Combine(outputFolder, relativePath);

                    if (isDirectory)
                    {
                        Directory.CreateDirectory(fullPath);
                        br.ReadInt64(); // skip size (0L)
                    }
                    else
                    {
                        long compressedSize = br.ReadInt64();
                        Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? outputFolder);

                        using (var outputStream = new FileStream(fullPath, FileMode.Create))
                        using (var bw = new BinaryWriter(outputStream))
                        {
                            var node = root;
                            long bytesWritten = 0;

                            while (bytesWritten < compressedSize)
                            {
                                bool bit = bitReader.ReadBit();
                                node = bit ? node.Right : node.Left;

                                if (node.IsLeaf)
                                {
                                    bw.Write(node.ByteValue.Value);
                                    bytesWritten++;
                                    node = root;
                                }
                            }
                        }
                    }

                    progress.Report((i + 1) * 100 / fileCount);
                }
            }
        }
    }
}
