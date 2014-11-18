using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RFD.SSO.Server.Model;
using RFD.SSO.Server.ServiceImpl;

namespace RFD.SSO.Server.Test
{
    [TestFixture]
    public class SSOServiceTest
    {
        [Test]
        public void GetLoginUrlTest()
        {
            SSOService sso = new SSOService();
            Assert.IsFalse(string.IsNullOrEmpty(sso.GetLoginUrl()));
        }

        [Test]
        public void GetNavigationBarTest()
        {
            SSOService sso = new SSOService();
            var bars = sso.GetNavigationBar();
            foreach (var s in bars)
            {
                Console.Out.WriteLine(string.Format("{0} --- {1}", s.WebAuthHandler, s.SiteName));
            }
        }

        [Test]
        public void ValidateTokenTest()
        {
            SSOService sso = new SSOService();

            SsoResponse ds = sso.ValidateToken("pms.wuliusys.com", "gvdmbygjtdicectoro5loyca", "vtXkzsC/KwszKy8tfGpCkX84h+8R76fkYgpPmSSaFkQ=");
        }
    }
}
