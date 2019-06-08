using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using IdStrings;


namespace IdStrings
{
    public class Creation
    {
        [Test]
        public void CreateOneIdString()
        {
            IdString a = new IdString("ss");
            Assert.IsFalse(a.IsNone());
        }

        [Test]
        public void CreateManyIdStrings()
        {
            IdString a = new IdString("ss");
            IdString b = new IdString("ii");
            Assert.IsFalse(a.IsNone());
            Assert.IsFalse(b.IsNone());
        }

        [Test]
        public void CanBeNone()
        {
            IdString a = new IdString("");
            Assert.IsTrue(a.IsNone());
        }
        
        [Test]
        public void CanBeCopied()
        {
            IdString a = new IdString("ss");
            IdString b = a;
            Assert.IsFalse(a.IsNone());
            Assert.IsFalse(b.IsNone());
            
            IdString c = "";
            b = c;
            Assert.IsTrue(c.IsNone());
            Assert.IsTrue(b.IsNone());
        }
    }
}
