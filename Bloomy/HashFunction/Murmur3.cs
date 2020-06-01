using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Bloomy.Lib.HashFunction
{
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
            ulong k1 = 0;

            while (i < value.Length)
            {
                byte[] data = value.Skip(i).Take(4).ToArray();
                streamLength += (uint)data.Length;
                switch (data.Length)
                {
                    case 4:
                        k1 = (ulong)
                           (data[0]
                          | data[1] << 8
                          | data[2] << 16
                          | data[3] << 24);

                        k1 *= C1;
                        k1 = Rotl64(k1, 15);
                        k1 *= C2;

                        final ^= k1;
                        final = Rotl64(final, 13);
                        final = final * 5 + C3;
                        break;
                    case 3:
                        k1 = (ulong)
                           (data[0]
                          | data[1] << 8
                          | data[2] << 16);
                        k1 *= C1;
                        k1 = Rotl64(k1, 15);
                        k1 *= C2;
                        final ^= k1;
                        break;
                    case 2:
                        k1 = (ulong)
                           (data[0]
                          | data[1] << 8);
                        k1 *= C1;
                        k1 = Rotl64(k1, 15);
                        k1 *= C2;
                        final ^= k1;
                        break;
                    case 1:
                        k1 = data[0];
                        k1 *= C1;
                        k1 = Rotl64(k1, 15);
                        k1 *= C2;
                        final ^= k1;
                        break;
                }
                i += 4;
            }

            final ^= streamLength;
            final = Fmix64(final);

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
