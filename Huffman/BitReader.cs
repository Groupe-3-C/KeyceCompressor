using System;
using System.IO;

namespace KeyceCompressor.Huffman
{
    internal class BitReader
    {
        private readonly Stream _stream;
        private int _buffer;
        private int _bitsLeft = 0;

        public BitReader(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            _stream = stream;
        }

        public bool ReadBit()
        {
            if (_bitsLeft == 0)
            {
                _buffer = _stream.ReadByte();
                if (_buffer == -1) throw new EndOfStreamException();
                _bitsLeft = 8;
            }
            _bitsLeft--;
            // Le bit le plus significatif est lu en premier
            return ((_buffer >> _bitsLeft) & 1) == 1;
        }
    }
}
