using Bloomy.Lib.HashFunction;
using System.Text;
using Xunit;

namespace Bloomy.Test
{
    public class HashTest
    {
        [Fact]
        public void Murmur3Characteristics()
        {
            var hashF = Murmur3.Create();
            var hash = hashF.ComputeHash(Encoding.ASCII.GetBytes("9089089whatevathisstringshouldbe123!!!"));
            Assert.Equal(8, hash.Length);
        }

        [Fact]
        public void Murmur3SimilarStrings()
        {
            var hash = Murmur3.Create();
            var hash1 = hash.ComputeHash(Encoding.ASCII.GetBytes("awesome1"));
            var hash2 = hash.ComputeHash(Encoding.ASCII.GetBytes("awesome1."));
            Assert.NotEqual(hash1, hash2);
        }
    }
}
