using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RFD.SSO.Server.Util
{
    public class SsoException : Exception
    {
        public SsoException()
        {
        }

        public SsoException(string message)
            : base(message)
        {
        }
    }
}
