using Bloomy.Lib.Filter;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Bloomy.Test
{
    public class FilterTest
    {
        public readonly string[] WordList = new string[] 
        { 
            "pasta", "subaru", "activist", "painting", "resident", "broken", "channel", "despite", "collision", "Peter",
            "cake", "take", "make", "bake", "rake", "Fake", "aake", "dake", "eake", "wake",
            "otorhinolaryngological", "radioimmunoelectrophoresis", "hepaticocholangiogastrostomy", "immunoelectrophoretically",
            "programmatically", "Thyroparathyroidectomized", "incomprehensibilities", "dichlorodifluoromethane",
            "password1234!", "BobAndMary$$56", "admin!_??", "asdjhbqiw74ey92498qyfawh82y60t298q", "1!!$%%auhsdu%$#$#@Uhffuh%^$",
            "$%(*&^%(*&Yiouhgasdo8089&%)*&U%)(dfagbdb*)O^&U)(*&%)(YH(O)UY%)#*(oh", "aoIHDO*Y*%^&)*HDALSJGH)(*%&)(DDSD"
        };

        [Fact]
        public void Murmur3FilterCheck()
        {
            BasicFilter filter = new BasicFilter(50, HashFunc.Murmur3);
            foreach(string str in WordList)
                filter.Insert(str);

            foreach(string str in WordList)
                Assert.True(filter.Check(str).Present);

            // This was brute-forced not to be in the set
            Assert.False(filter.Check("notInThere4").Present);
        }

        [Fact]
        public void SHA256FilterCheck()
        {
            BasicFilter filter = new BasicFilter(50, HashFunc.SHA256);
            foreach (string str in WordList)
                filter.Insert(str);

            foreach (string str in WordList)
                Assert.True(filter.Check(str).Present);

            // This was brute-forced not to be in the set
            Assert.False(filter.Check("notInThere4").Present);

            // This was brute-forced to have matching bits although it's not in the set
            Assert.True(filter.Check("notInThere11").Present);
        }

        [Fact]
        public void SHA512FilterCheck()
        {
            BasicFilter filter = new BasicFilter(50, HashFunc.SHA512);
            foreach (string str in WordList)
                filter.Insert(str);

            foreach (string str in WordList)
                Assert.True(filter.Check(str).Present);

            // This was brute-forced not to be in the set
            Assert.False(filter.Check("notInThere4").Present);

            // This was brute-forced to have matching bits although it's not in the set
            Assert.True(filter.Check("notInThere11").Present);
        }
    }
}
