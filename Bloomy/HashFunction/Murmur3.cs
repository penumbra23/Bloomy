using System;
using System.Linq;

namespace Bloomy.Lib.HashFunction
{
    /// <summary>
    /// Implementation of the Murmur3 hash function.
    /// </summary>
    public class Murmur3 : IHashProvider
    {
        const int Seed = 144;
        const uint C1 = 0xcc9e2d51;
        const uint C2 = 0x1b873593;
        const uint C3 = 0xe6546b64;

        internal Murmur3() { }

        public static IHashProvider Create() => new Murmur3();

        public byte[] ComputeHash(byte[] value)
        {
            ulong final = Seed;
            uint streamLength = 0;
            int i = 0;
            ulong tmp = 0;

            while (i < value.Length)
            {
                byte[] data = value.Skip(i).Take(8).ToArray();
                streamLength += (uint)data.Length;
                switch (data.Length)
                {
                    case 4:
                        tmp = (ulong)(data[0] | data[1] << 8  | data[2] << 16 | data[3] << 24) * C1;
                        tmp = Rotl64(tmp, 15) * C2;
                        final ^= tmp;
                        final = Rotl64(final, 13) * 5 + C3;
                        break;
                    case 3:
                        tmp = (ulong)(data[0] | data[1] << 8 | data[2] << 16) * C1;
                        tmp = Rotl64(tmp, 15) * C2;
                        final ^= tmp;
                        break;
                    case 2:
                        tmp = (ulong)(data[0] | data[1] << 8) * C1;
                        tmp = Rotl64(tmp, 15) * C2;
                        final ^= tmp;
                        break;
                    case 1:
                        tmp = data[0] * C1;
                        tmp = Rotl64(tmp, 15) * C2;
                        final ^= tmp;
                        break;
                }
                // Move 8 bytes forward
                i += 8;
            }

            final = Fmix64(final ^ streamLength);

            return BitConverter.GetBytes(final);
        }

        private ulong Rotl64(ulong x, byte r) => (x << r) | (x >> (64 - r));

        private ulong Fmix64(ulong h)
        {
            h ^= h >> 16;
            h *= 0x85ebca6b;
            h ^= h >> 13;
            h *= 0xc2b2ae35;
            h ^= h >> 16;
            return h;
        }
    }
}
