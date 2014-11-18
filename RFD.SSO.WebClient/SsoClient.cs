using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using log4net;

namespace RFD.SSO.WebClient
{
    public class SsoClient
    {
        private SSOServiceClient _client;

        public SsoClient()
        {
            string endpointName = (IsDemo() ? "WSHttpBinding_ISSOService_demo" : "WSHttpBinding_ISSOService");
            _client = new SSOServiceClient(endpointName);
        }

        private bool IsDemo()
        {
            try
            {
                string host = HttpContext.Current.Request.Url.Host.ToLower();

                return (host.Contains("demo.wuliusys.com") || !host.Contains(".wuliusys.com"));
            }
            catch (Exception ex)
            {
                MessageCollector.Instance.Collect(GetType(), "判断是否是demo系统失败：" + ex.ToString());
            }
            return true;
        }

        public bool Login(string userId, string token, IProcessLoginInfo processLoginInfo)
        {
            if (string.IsNullOrEmpty(userId))
            {
                MessageCollector.Instance.Collect(GetType(), "userId为空");
                return false;
            }

            if (string.IsNullOrEmpty(token))
            {
                MessageCollector.Instance.Collect(GetType(), "token为空");
                return false;
            }

            var ssoResponse = ValidateToken(token);
            if (ssoResponse == null)
            {
                MessageCollector.Instance.Collect(GetType(), "Login_ValidateToken验证失败");
                return false;
            }

            ClientOnlineUser.Instance.Add(userId, token);
            return SetCurrentUser(userId, processLoginInfo, ssoResponse);
        }

        public string QueryTokenFromUrl()
        {
            return HttpContext.Current.Request.QueryString[SsoUrlName.SSOTOKEN];
        }

        public string QueryUidFromUrl()
        {
            return HttpContext.Current.Request.QueryString[SsoUrlName.SSOUID];
        }

        private SsoResponse ValidateToken(string token)
        {
            string ipAddress = GetIpAddress();
            string siteId = GetSiteId();

            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            return _client.ValidateToken(siteId, ipAddress, token);
        }

        public bool UserExist(string userId)
        {
            return ClientOnlineUser.Instance.Exists(userId);
        }

        public bool IsCurrentUser(string userId)
        {
            if (!string.IsNullOrEmpty(userId) && userId == ClientOnlineUser.Instance.CurrentUserId)
            {
                return true;
            }
            return false;
        }

        public void Logout()
        {
            if (!LoginType.IsSsoLogin)
            {
                return;
            }

            try
            {
                string token = ClientOnlineUser.Instance.CurrentSsoToken;
                string siteId = GetSiteId();

                if (!string.IsNullOrEmpty(siteId) && !string.IsNullOrEmpty(token))
                {
                    _client.Logout(siteId, token);
                }
            }
            finally
            {
                ClientOnlineUser.Instance.Remove(ClientOnlineUser.Instance.CurrentUserId);
            }
        }

        private bool SetCurrentUser(string userId, IProcessLoginInfo processLoginInfo, SsoResponse ssoResponse)
        {
            if (null == ssoResponse)
            {
                var token = ClientOnlineUser.Instance.GetSsoToken(userId);
                ssoResponse = ValidateToken(token);

                if (null == ssoResponse)
                {
                    MessageCollector.Instance.Collect(GetType(), "SetCurrentUser_ValidateToken失败");
                    return false;
                }
            }

            ClientOnlineUser.Instance.CurrentUserId = userId;
            if (processLoginInfo != null)
            {
                processLoginInfo.ProcessLoginInfo(ssoResponse);
            }
            return true;
        }

        public bool SetCurrentUser(string userId, IProcessLoginInfo processLoginInfo)
        {
            return SetCurrentUser(userId, processLoginInfo, null);
        }

        public string GetNavigationHtml()
        {
            string navi = string.Empty;
            if (LoginType.IsSsoLogin)
            {
                var sites = GetSiteUrlList();

                foreach (var site in sites)
                {
                    navi += string.Format(@"<li><a target='_blank' href='{0}?{1}={2}&{3}={4}'>{5}</a></li>",
                        site.WebAuthHandler,
                        SsoUrlName.SSOTOKEN,
                        HttpContext.Current.Server.UrlEncode(ClientOnlineUser.Instance.CurrentSsoToken),
                        SsoUrlName.SSOUID,
                        HttpContext.Current.Server.UrlEncode(ClientOnlineUser.Instance.CurrentUserId),
                        site.SiteName);
                }
            }
            return navi;
        }

        public List<Navigation> GetSiteUrlList()
        {
            try
            {
                return _client.GetNavigationBar().ToList();
            }
            catch
            {
            }
            return new List<Navigation>();
        }

        public void ReURL()
        {
            var ssoUid = HttpContext.Current.Request.QueryString[SsoUrlName.SSOUID];
            if (!string.IsNullOrEmpty(ssoUid))
            {
                return;
            }

            if (HttpContext.Current.Request.UrlReferrer == null ||
                HttpContext.Current.Request.UrlReferrer.ToString().IndexOf(SsoUrlName.SSOUID + "=") < 0)
            {
                ssoUid = ClientOnlineUser.Instance.CurrentUserId;

            }
            else
            {
                ResolveUrl reUrl = new ResolveUrl();
                ssoUid = reUrl.Querystring(HttpContext.Current.Request.UrlReferrer.ToString())[SsoUrlName.SSOUID];
            }

            if (string.IsNullOrEmpty(ssoUid))
            {
                return;
            }

            string urlMark = HttpContext.Current.Request.Url.ToString().IndexOf("?") > -1 ? string.Empty : "?";
            HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.ToString() + urlMark +
                string.Format("&{0}={1}", SsoUrlName.SSOUID, ssoUid), true);
        }

        /// <summary>
        /// 跳转的登录页
        /// </summary>
        public void RedirectToLoginPage()
        {
            string loginUrl = _client.GetLoginUrl();
            string ipAddress = GetIpAddress();
            string siteId = GetSiteId();
            string reUrl = string.Format("{0}?siteId={1}&ip={2}", loginUrl, siteId, ipAddress);

            //下面的代码是为了方便调试
            var request = HttpContext.Current.Request;
            if (request.Url.ToString().StartsWith("http://localhost"))
            {
                var backurl = "http://" + request.Url.Authority;
                reUrl = reUrl + string.Format("&backurl={0}", backurl);
            }

            HttpContext.Current.Response.Redirect(reUrl);
        }

        public void RedirectToLoginPage(string backurl)
        {
            string loginUrl = _client.GetLoginUrl();
            string ipAddress = GetIpAddress();
            string siteId = GetSiteId();
            string reUrl = string.Format("{0}?siteId={1}&ip={2}", loginUrl, siteId, ipAddress);

            //下面的代码是为了方便调试
            var request = HttpContext.Current.Request;

            //var backurl = "http://" + request.Url.Authority;
            reUrl = reUrl + string.Format("&backurl={0}", backurl);


            HttpContext.Current.Response.Redirect(reUrl);
        }

        private string GetSiteId()
        {
            return ConfigurationManager.AppSettings["ssoAuthsiteId"].ToLower();
        }

        /// <summary>
        /// 获得发出请求的远程主机的 IP 地址
        /// </summary>
        /// <returns></returns>
        private string GetIpAddress()
        {
            return "abcd";// HttpContext.Current.Session.SessionID;

            string ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(ipAddress))
            {
                throw new Exception("无法获得发出请求的远程主机的IP");
            }

            return ipAddress;
        }
    }

    public class ResolveUrl
    {
        public Dictionary<string, string> Querystring(string url)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (var p in url.Substring(url.IndexOf('?') + 1).Split('&').ToList())
            {
                if (p.Contains("="))
                {
                    dic.Add(p.Substring(0, p.IndexOf("=")), p.Substring(p.IndexOf("=") + 1));
                }
            }

            return dic;
        }
    }

    /// <summary>
    /// 客户端在线用户，用于多用户
    /// </summary>
    public class ClientOnlineUser
    {
        //private readonly static ClientOnlineUser _instance;
        private static ClientOnlineUser _instance;

        //static ClientOnlineUser()
        //{
        //    _instance = new ClientOnlineUser();
        //}

        private ClientOnlineUser()
        {
        }

        public static ClientOnlineUser Instance
        {
            get
            {
                return _instance ?? (_instance = new ClientOnlineUser());
            }
        }

        public void Add(string userId, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("token为空");
            }

            var ssouserList = GetSsoUsers();

            if (!ssouserList.Exists(u => u.StartsWith(RedefineUserId(userId))))
            {
                ssouserList.Add(RedefineUserId(userId) + token);
            }

            SaveSsoUsers(ssouserList);
        }

        public void Remove(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return;
            }

            var ssouserList = GetSsoUsers();

            if (ssouserList.Exists(u => u.StartsWith(RedefineUserId(userId))))
            {
                ssouserList.Remove(RedefineUserId(userId));
            }

            SaveSsoUsers(ssouserList);
        }

        public bool Exists(string userId)
        {
            var ssouserList = GetSsoUsers();

            return ssouserList.Exists(u => u.StartsWith(RedefineUserId(userId)));
        }

        public string GetSsoToken(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return "";
            }

            var ssouserList = GetSsoUsers();

            string com = ssouserList.FirstOrDefault(u => u.StartsWith(RedefineUserId(userId)));

            return string.IsNullOrEmpty(com) ? "" : com.Substring(RedefineUserId(userId).Length);
        }

        public string CurrentUserId
        {
            get
            {
                return SsoCookieUtil.ExistCookie(SsoCookieName.SSO_UID) ? SsoCookieUtil.GetCookie(SsoCookieName.SSO_UID) : "";
            }
            set
            {
                SsoCookieUtil.AddCookie(SsoCookieName.SSO_UID, value, 18);
            }
        }

        public string CurrentSsoToken
        {
            get
            {
                return GetSsoToken(CurrentUserId);
            }
        }

        private List<string> GetSsoUsers()
        {
            string ssousers = (HttpContext.Current.Request.Cookies["ssousers"] == null)
                                 ? string.Empty
                                 : SsoCookieUtil.GetCookie("ssousers");

            List<string> ssouserList = ssousers.Split(';').ToList();
            ssouserList.RemoveAll(string.IsNullOrEmpty);

            return ssouserList;
        }

        /// <summary>
        /// 加工userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private string RedefineUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("用户ID为空");
            }

            return userId + ",";
        }

        private void SaveSsoUsers(List<string> ssouserList)
        {
            string ssousers = string.Empty;
            foreach (string u in ssouserList)
            {
                ssousers += string.IsNullOrEmpty(u) ? "" :
                    ((string.IsNullOrEmpty(ssousers) ? "" : ";") + u);
            }

            if (!string.IsNullOrEmpty(ssousers))
            {
                SsoCookieUtil.AddCookie("ssousers", ssousers, 18);
            }
        }
    }

    public class SsoCookieName
    {
        public const string SSO_TOKEN = "ssoToken";
        public const string SSO_UID = "ssoUid";
    }

    public class SsoUrlName
    {
        public const string SSOTOKEN = "ssotoken";
        public const string SSOUID = "ssouid";
    }

    public class MessageCollector
    {
        private string _ip = string.Empty;
        private static MessageCollector _instance = null;

        public static MessageCollector Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new MessageCollector();
                }
                return _instance;
            }
        }

        private MessageCollector()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public void Collect(Type type, string msg)
        {
            LogManager.GetLogger(type).Error(msg);
        }

        public void Collect(string key, string msg)
        {
            LogManager.GetLogger(key).Error(msg);
        }
    }

    /// <summary>
    /// 登录方式
    /// </summary>
    public class LoginType
    {
        private const string _ssoLoginYn = "ssoLoginYn";
        /// <summary>
        /// 是否是云平台登录
        /// </summary>
        public static bool IsSsoLogin
        {
            get
            {
                try
                {
                    return SsoCookieUtil.ExistCookie(_ssoLoginYn) && (SsoCookieUtil.GetCookie(_ssoLoginYn).ToUpper() == "Y");
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                SsoCookieUtil.AddCookie(_ssoLoginYn, (value ? "Y" : "N"), 18);
            }
        }
    }
}
