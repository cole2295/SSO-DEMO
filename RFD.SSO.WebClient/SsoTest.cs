using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using NUnit.Framework;


namespace RFD.SSO.WebClient
{
    [TestFixture]
    public class SsoTest
    {
        [Test]
        public void ssoGetUrlTest()
        {
            SSOServiceClient sso = new SSOServiceClient();

            var ulr = sso.GetLoginUrl();
            Assert.IsTrue(string.IsNullOrEmpty(ulr) == false);
        }

        [Test]
        public void LoginTest()
        {
            try
            {
                SSOServiceClient client = new SSOServiceClient();


                var ds = client.ValidateToken("pms.wuliusys.com", "120", "vtXkzsC/KwszKy8tfGpCkX84h+8R76fkYgpPmSSaFkQ=");

            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.ToString());
            }
        }

        [Test]
        public void IsDemoTest()
        {
            Assert.IsTrue(IsDemo(""));
            Assert.IsTrue(IsDemo("www.vancl.com"));
            Assert.IsTrue(IsDemo("lmsdemo.wuliusys.com"));
            Assert.IsFalse(IsDemo("lms.wuliusys.com"));

        }

        private bool IsDemo(string host)
        {
            return (host.Contains("demo.wuliusys.com") || !host.Contains(".wuliusys.com"));
        }
    }
}
