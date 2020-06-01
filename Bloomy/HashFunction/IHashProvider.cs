namespace Bloomy.Lib.HashFunction
{
    public interface IHashProvider
    {
        byte[] ComputeHash(byte[] value);
    }
}
