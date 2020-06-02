using Bloomy.Lib.HashFunction;
using Bloomy.Lib.HashFunction.SHA;
using System;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Bloomy.Lib.Filter
{
    public class BasicFilter
    {
        private readonly byte[] prefix = new byte[] { 0x00, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2A };

        public BasicFilter(int width, HashFunc hashFunction = HashFunc.SHA256, ushort hashNumber = 3)
        {
            Width = width;
            HashFunction = hashFunction;
            Filter = new byte[(uint)Math.Ceiling((double)Width / 8)];
            HashNumber = (uint)Math.Min(hashNumber, prefix.Length);
        }

        private byte[] Filter { get; set; }

        private uint Count { get; set; }

        private uint HashNumber { get; }

        public int Width { get; }

        public HashFunc HashFunction { get; }

        public void Insert(string value)
        {
            for(int i = 0; i < HashNumber; ++i)
            {
                (uint byteNo, uint bitPos) = calcBitPosition(value, i);
                Filter[byteNo] = (byte)(Filter[byteNo] | (1 << (int)(7 - bitPos % 8)));
            }
            Count++;
        }

        public FilterResult Check(string value)
        {
            for (int i = 0; i < HashNumber; ++i)
            {
                (uint byteNo, uint bitPos) = calcBitPosition(value, i);
                var mask = Filter[byteNo] & (1 << (int)(7 - bitPos % 8));
                if (mask == 0)
                    return new FilterResult(false, 1);
            }

            return new FilterResult(true, Math.Pow((1 - Math.Exp(-(double)HashNumber * Count / Width)), HashNumber));
        }

        private IHashProvider CreateHashAlgorithm(HashFunc func)
        {
            switch(func)
            {
                case HashFunc.SHA256:
                    return SHA256Provider.Create();
                case HashFunc.SHA512:
                    return SHA512Provider.Create();
                case HashFunc.Murmur3:
                    return Murmur3.Create();
                default:
                    throw new NotImplementedException();
            }
        }

        private BigInteger calcBigInteger(string value, int i)
        {
            var hash = CreateHashAlgorithm(HashFunction);
            var hashBytes = Encoding.ASCII.GetBytes(value);
            hashBytes[0] += prefix[i];
            byte[] bytes = hash.ComputeHash(hashBytes).Append((byte)0x0).ToArray();
            return new BigInteger(bytes);
        }

        private (uint, uint) calcBitPosition(string value, int i)
        {
            BigInteger num = calcBigInteger(value, i);
            uint bitPos = (uint)(num % Width);
            uint byteNo = bitPos / 8;
            return (byteNo, bitPos);
        }
    }
}
