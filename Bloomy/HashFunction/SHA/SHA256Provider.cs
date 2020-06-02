using System.Security.Cryptography;

namespace Bloomy.Lib.HashFunction.SHA
{
    internal class SHA256Provider : IHashProvider
    {
        internal SHA256Provider()
        {
            Instance = SHA256.Create();
        }

        protected SHA256 Instance { get; }

        public byte[] ComputeHash(byte[] value)
        {
            return Instance.ComputeHash(value);
        }

        public static IHashProvider Create()
        {
            return new SHA256Provider();
        }
    }
}
