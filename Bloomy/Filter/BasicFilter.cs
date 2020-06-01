using Bloomy.Lib.HashFunction;
using Bloomy.Lib.HashFunction.SHA;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace Bloomy.Lib.Filter
{
    public class BasicFilter
    {
        public BasicFilter(int width, HashFunc hashFunction = HashFunc.SHA256)
        {
            Width = width;
            HashFunction = hashFunction;
        }

        public int Width { get; }

        public HashFunc HashFunction { get; }

        public void Insert(string value)
        {
            var hash = CreateHashAlgorithm(HashFunction);
            var bytes = hash.ComputeHash(Encoding.ASCII.GetBytes(value));
            BigInteger num = new BigInteger(bytes);
            // TODO: implement filter insertion
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
    }
}
