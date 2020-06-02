using System;
using System.Security.Cryptography;

namespace Bloomy.Lib.HashFunction.SHA
{
    internal class SHA512Provider : IHashProvider
    {
        internal SHA512Provider()
        {
            Instance = SHA512.Create();
        }

        protected SHA512 Instance { get; }

        public byte[] ComputeHash(byte[] value)
        {
            return Instance.ComputeHash(value);
        }

        public static IHashProvider Create()
        {
            return new SHA512Provider();
        }
    }
}
