using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Moq;
using NUnit.Framework;

namespace ControlButton
{
    [TestFixture]
    public class HandlerLoginTest
    {
        [Test]
        public void ProcessRequestTest()
        {
            var httpContextStub = new Mock<HttpContextBase>
            {

                DefaultValue = DefaultValue.Mock

            };

            HandlerLogin hl = new HandlerLogin();

           // hl.ProcessRequest(httpContextStub);
        }
    }
}