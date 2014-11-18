using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LMS.Util;
using NUnit.Framework;

namespace RFD.SSO.Server.Test
{
    [TestFixture]
    public class BaseTypeExtensionTest
    {
        [Test]
        public void IntTest()
        {
            int? a = null;
            Assert.AreEqual(0, a.ToString().TryGetInt());
        }
    }
}
