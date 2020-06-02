using Bloomy.Lib.HashFunction;
using Bloomy.Lib.HashFunction.SHA;
using System;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Bloomy.Lib.Filter
{
    /// <summary>
    /// Basic bloom filter of any bit length.
    /// </summary>
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

        /// <summary>
        /// Bloom filter bit array.
        /// </summary>
        private byte[] Filter { get; set; }

        /// <summary>
        /// Number of elements inserted.
        /// </summary>
        private uint Count { get; set; }

        /// <summary>
        /// Number of different hash values to use (correlates to the number of hash functions).
        /// </summary>
        public uint HashNumber { get; }

        /// <summary>
        /// Bit-length of the bloom filter.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Hash function which is used on inserting <see cref="HashFunc"/>.
        /// </summary>
        public HashFunc HashFunction { get; }

        /// <summary>
        /// Inserts a value inside the filter.
        /// </summary>
        /// <param name="value">String value to insert in the filter.</param>
        public void Insert(string value)
        {
            for(int i = 0; i < HashNumber; ++i)
            {
                (uint byteNo, uint bitPos) = calcBitPosition(value, i);
                Filter[byteNo] = (byte)(Filter[byteNo] | (1 << (int)(7 - bitPos % 8)));
            }
            Count++;
        }

        /// <summary>
        /// Checks if the given value might be or isn't definitely inside the filter.
        /// </summary>
        /// <param name="value">String value to check.</param>
        /// <returns></returns>
        public FilterResult Check(string value)
        {
            for (int i = 0; i < HashNumber; ++i)
            {
                (uint byteNo, uint bitPos) = calcBitPosition(value, i);
                var mask = Filter[byteNo] & (1 << (int)(7 - bitPos % 8));
                if (mask == 0)
                    return new FilterResult(BloomPresence.NotInserted, 1);
            }

            return new FilterResult(BloomPresence.MightBeInserted, Math.Pow((1 - Math.Exp(-(double)HashNumber * Count / Width)), HashNumber));
        }

        /// <summary>
        /// <see cref="IHashProvider"/> factory.
        /// </summary>
        /// <param name="func">Hash function to use; see <see cref="HashFunc"/>.</param>
        /// <returns></returns>
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
