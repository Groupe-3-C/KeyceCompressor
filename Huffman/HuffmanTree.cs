using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;


namespace KeyceCompressor.Huffman
{
    internal class HuffmanTree
    {
        private readonly HuffmanNode root;

        public HuffmanTree(Dictionary<byte, long> freq)
        {
            var nodes = new List<HuffmanNode>();
            foreach (var p in freq)
                nodes.Add(new HuffmanNode { ByteValue = p.Key, Frequency = p.Value });

            if (nodes.Count == 0) { root = null; return; }

            while (nodes.Count > 1)
            {
                nodes = nodes.OrderBy(n => n.Frequency).ToList();
                var a = nodes[0];
                var b = nodes[1];
                nodes.RemoveRange(0, 2);
                var parent = new HuffmanNode
                {
                    Frequency = a.Frequency + b.Frequency,
                    Left = a,
                    Right = b
                    // ByteValue stays null -> IsLeaf will be false
                };
                nodes.Add(parent);
            }

            root = nodes.Count > 0 ? nodes[0] : null;
        }

        public Dictionary<byte, string> GetCodes()
        {
            var codes = new Dictionary<byte, string>();
            if (root != null) BuildCodes(root, "", codes);
            return codes;
        }

        private void BuildCodes(HuffmanNode node, string code, Dictionary<byte, string> codes)
        {
            if (node.IsLeaf)
            {
                codes[node.ByteValue.Value] = code == "" ? "0" : code;
                return;
            }
            if (node.Left != null) BuildCodes(node.Left, code + "0", codes);
            if (node.Right != null) BuildCodes(node.Right, code + "1", codes);
        }

        public void Serialize(BinaryWriter bw) => SerializeNode(root, bw);
        private void SerializeNode(HuffmanNode node, BinaryWriter bw)
        {
            if (node == null) return;
            bw.Write(node.IsLeaf);
            if (node.IsLeaf) bw.Write(node.ByteValue.Value);
            SerializeNode(node.Left, bw);
            SerializeNode(node.Right, bw);
        }

        public static HuffmanNode Deserialize(BinaryReader br)
        {
            bool leaf = br.ReadBoolean();
            if (leaf) return new HuffmanNode { ByteValue = br.ReadByte() };
            var left = Deserialize(br);
            var right = Deserialize(br);
            return new HuffmanNode { Left = left, Right = right };
        }

        public HuffmanNode Root => root;
    }
}
