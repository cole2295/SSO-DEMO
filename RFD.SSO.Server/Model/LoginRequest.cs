using System;
using System.Runtime.Serialization;

namespace RFD.SSO.Server.Model
{
    [Serializable]
    [DataContract]
    public class LoginRequest
    {
        /// <summary>
        /// 站点
        /// </summary>
        [DataMember]
        public string WebSite
        {
            get;
            set;
        }

        /// <summary>
        /// 发起请求的客户端的IP
        /// </summary>
        [DataMember]
        public string IP
        {
            get;
            set;
        }

        /// <summary>
        /// 登录名
        /// </summary>
        [DataMember]
        public string LoginId
        {
            get;
            set;
        }

        /// <summary>
        /// 密码
        /// </summary>
        [DataMember]
        public string Password
        {
            get;
            set;
        }
    }
}
