using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RFD.SSO.Server.Model
{
    [Serializable]
    [DataContract]
    public class Navigation
    {
        /// <summary>
        /// 站点处理认证页面
        /// </summary>
        [DataMember]
        public string WebAuthHandler
        {
            get;
            set;
        }

        /// <summary>
        /// 站点名称
        /// </summary>
        [DataMember]
        public string SiteName
        {
            get;
            set;
        }
    }
}
