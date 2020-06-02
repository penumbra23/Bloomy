namespace Bloomy.Lib.HashFunction
{
    /// <summary>
    /// Hashing interface for the bloom filter hash function.
    /// </summary>
    public interface IHashProvider
    {
        byte[] ComputeHash(byte[] value);
    }
}
