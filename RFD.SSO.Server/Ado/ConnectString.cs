using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using LMS.Util.Security;

namespace RFD.SSO.Server.Ado
{
    public static class ConnectString
    {
        private static string _connstr;
        private static string _lmsconnstr;

        public static string PmsReadOnlyConStr
        {
            get
            {
                if (string.IsNullOrEmpty(_connstr))
                {
                    _connstr = DES.Decrypt3DES(ConfigurationManager.ConnectionStrings["PMSReadOnlyConnString"].ToString().Trim());
                }

                return _connstr;
            }
        }

        public static string LmsConStr
        {
            get
            {
                if (string.IsNullOrEmpty(_lmsconnstr))
                {
                    _lmsconnstr = DES.Decrypt3DES(ConfigurationManager.ConnectionStrings["LMSConnString"].ToString().Trim());
                }

                return _lmsconnstr;
            }
        }
    }
}
