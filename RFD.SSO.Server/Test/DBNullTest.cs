using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;


namespace RFD.SSO.Server.Test
{
    [TestFixture]
    public class DBNullTest
    {
        [Test]
        public void Test()
        {
            DBNull.Value.ToString();
        }

    }
}
