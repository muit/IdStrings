using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using IdStrings;


namespace IdStrings
{
    public class Comparisons
    {
        [Test]
        public void CanCompareAndSucceed()
        {
            IdString a = new IdString("ss");
            IdString b = new IdString("ss");
            Assert.AreEqual(a, b);
        }

        [Test]
        public void CanCompareAndFail()
        {
            IdString a = new IdString("ss");
            IdString b = new IdString("nn");
            Assert.AreNotEqual(a, b);
        }
        
        [Test]
        public void CanCompareNone()
        {
            IdString a = new IdString("ss");
            IdString b = new IdString("");
            Assert.AreNotEqual(a, b);
        }

        [Test]
        public void CanGetString()
        {
            IdString a = new IdString("ss");
            Assert.AreEqual("ss", (string)a);
        }
    }
}
