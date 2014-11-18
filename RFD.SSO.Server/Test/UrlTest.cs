using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace RFD.SSO.Server.Test
{
    [TestFixture]
    public class UrlTest
    {
        [Test]
        public void Url()
        {
            UriBuilder u = new UriBuilder("http://localhost:12233/SsoAuthHandler.aspx");
            Assert.AreEqual("/SsoAuthHandler.aspx", u.Path);

            u = new UriBuilder("http://tms.wuliusys.com/Home/ssoLogin");
            Assert.AreEqual("/Home/ssoLogin", u.Path);
        }
    }
}
