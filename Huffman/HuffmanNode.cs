using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyceCompressor.Huffman
{
    internal class HuffmanNode : IComparable<HuffmanNode>
    {
        public byte? ByteValue { get; set; }
        public long Frequency { get; set; }
        public HuffmanNode Left { get; set; }
        public HuffmanNode Right { get; set; }
        public bool IsLeaf => ByteValue.HasValue;

        public int CompareTo(HuffmanNode other)
        {
            int result = Frequency.CompareTo(other.Frequency);
            if (result == 0 && IsLeaf && other.IsLeaf)
            {
                // Pour une construction déterministe de l'arbre
                return ByteValue.Value.CompareTo(other.ByteValue.Value);
            }
            return result;
        }
    }
}
