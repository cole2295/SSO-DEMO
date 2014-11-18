namespace RFD.SSO.Server.Model
{
    public class Site
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public string SiteId
        {
            get;
            set;
        }

        /// <summary>
        /// 站点编号
        /// </summary>
        public string SiteCode { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public string Usable
        {
            get;
            set;
        }

        /// <summary>
        /// 站点加密公钥
        /// </summary>
        public string PublicKey
        {
            get;
            set;
        }

        /// <summary>
        /// 站点加密方式
        /// </summary>
        public string PublicKeyMode
        {
            get;
            set;
        }

        /// <summary>
        /// 站点处理认证页面
        /// </summary>
        public string WebAuthHandler
        {
            get;
            set;
        }

        /// <summary>
        /// 站点名称
        /// </summary>
        public string SiteName
        {
            get;
            set;
        }

        /// <summary>
        /// 站点照片
        /// </summary>
        public string Pic
        {
            get;
            set;
        }
    }
}
