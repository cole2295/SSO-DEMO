using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RFD.SSO.Server.Service;

namespace RFD.SSO.Server.Test
{
    [TestFixture]
    public class DSATest
    {
        [Test]
        public void EncryptTest()
        {
            DSA das = new DSA();
            das.Encrypt("a", "");
        }

        [Test]
        public void DecryptTest()
        {
            DSA das = new DSA();
            das.Decrypt("", "");
        }
    }
}
