using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RFD.SSO.Server.ServiceImpl;

namespace RFD.SSO.Server.Test
{
    [TestFixture]
    public class MyConfigurationManagerTest
    {
        [Test]
        public void AppSettingsTest()
        {
            Assert.AreEqual("Server=127.0.0.1", MyConfigurationManager.Instance.AppSettings("MongoDb.config")["connectionString"]);
            Assert.AreEqual("RfdSso", MyConfigurationManager.Instance.AppSettings("MongoDb.config")["dbname"]);
        }
    }
}
