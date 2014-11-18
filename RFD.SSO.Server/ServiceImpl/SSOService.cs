using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.ServiceModel;
using System.Xml;
using RFD.SSO.Server.Dao;
using System.Configuration;
using RFD.SSO.Server.Domain;
using RFD.SSO.Server.Model;
using RFD.SSO.Server.Service;
using RFD.SSO.Server.ServiceImpl;

namespace RFD.SSO.Server.ServiceImpl
{
    // 注意: 如果更改此处的类名 "SSOService"，也必须更新 Web.config 中对 "SSOService" 的引用。
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, IncludeExceptionDetailInFaults = true, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class SSOService : ISSOService
    {
        private IUserDao _userDod = new UserDao();
        public string Login(LoginRequest loginRequest)
        {
            //try
            //{
            //ValidUserList.Instance.UserDao = new UserDao();
            //ValidUserList.Instance.DB = new MongoDb(); // FileDB();

            string token = ValidUserList.Instance.Login(loginRequest);

            MessageCollector.Instance.Collect(GetType(), string.Format("登录{0}siteId:{1} \r\n ip:{2} \r\n token:{3}", (string.IsNullOrEmpty(token) ? "失败" : "成功"), loginRequest.WebSite, loginRequest.IP, token));
            return token;
            //}
            //catch (Exception ex)
            //{
            //    MessageCollector.Instance.Collect(GetType(), string.Format("登录异常:{0}\r\nMessage:{1}\r\nSource:{2}\r\nTrace:{3} \r\n", "ValidateToken()", ex.Message, ex.Source, ex.StackTrace));
            //}
            //return string.Empty;
        }

        public void Logout(string siteId, string token)
        {
            try
            {
                ValidUserList.Instance.Logout(siteId, token);

                MessageCollector.Instance.Collect(GetType(), string.Format("退出成功siteId:{0} \r\n token:{1}", siteId, token));
            }
            catch (Exception ex)
            {
                MessageCollector.Instance.Collect(GetType(), string.Format("退出异常:{0}\r\nMessage:{1}\r\nSource:{2}\r\nTrace:{3} \r\n", "ValidateToken()", ex.Message, ex.Source, ex.StackTrace));
            }
        }

        public SsoResponse ValidateToken(string siteId, string ip, string token)
        {
            try
            {
                SsoResponse ssoResponse;

                bool ok = ValidUserList.Instance.ValidateToken(siteId, ip, token, out ssoResponse);

                if (ok)
                {
                    MessageCollector.Instance.Collect(GetType(), string.Format("验证成功siteId:{0} \r\n ip:{1} \r\n token:{2}", siteId, ip, token));
                    return ssoResponse;
                }
            }
            catch (Exception ex)
            {
                MessageCollector.Instance.Collect(GetType(), string.Format("验证异常 \r\n siteId:{0} \r\n ip:{1} \r\n token:{2} \r\n Exception:{3} \r\n ", siteId, ip, token, ex));
            }

            MessageCollector.Instance.Collect(GetType(), string.Format("验证失败 siteId:{0} \r\n ip:{1} \r\n token:{2}", siteId, ip, token));
            return null;
        }

        public string GetLoginUrl()
        {
            try
            {
                return SiteList.Instance.LoginUrl;
            }
            catch (Exception ex)
            {
                MessageCollector.Instance.Collect(GetType(), ex.ToString());
            }
            return "";
        }

        public string GetWebAuthHandler(string siteId)
        {
            try
            {
                var site = SiteList.Instance.GetSiteInfo(siteId);
                return site.WebAuthHandler;
            }
            catch (Exception ex)
            {
                MessageCollector.Instance.Collect(GetType(), ex.ToString());
            }
            return "";
        }

        public List<Navigation> GetNavigationBar()
        {
            try
            {
                List<Navigation> navs = new List<Navigation>();

                var siteList = SiteList.Instance.UsableSites;
                foreach (var s in siteList)
                {
                    navs.Add(new Navigation
                    {
                        SiteName = s.SiteName,
                        WebAuthHandler = s.WebAuthHandler
                    });
                }
                return navs;
            }
            catch (Exception ex)
            {
                MessageCollector.Instance.Collect(GetType(), ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// 根据用户ID获取菜单
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataSet GetMenuListByUserID(string UserID)
        {
            return _userDod.GetMenuListByUserID(UserID);
        }
    }
}
