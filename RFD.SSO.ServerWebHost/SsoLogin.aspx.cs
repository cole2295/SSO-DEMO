using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using LMS.Util;
using RFD.SSO.Server.Model;
using RFD.SSO.Server.ServiceImpl;
using RFD.SSO.Server.Dao;
using RFD.SSO.Server.Util;


namespace RFD.SSO.ServerWebHost
{
    public partial class SsoLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hidSiteId.Value = Request.QueryString["siteId"];
                hidIP.Value = Request.QueryString["ip"];
                hidBackUrl.Value = Request.QueryString["backurl"];
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = GetFormValue("txtUserName");
                string password = GetFormValue("Password");
                string verificationCode = GetFormValue("VerificationCode");

                CookieUtil.AddCookie("ChangeValidate", "NO");
                if (Request.Cookies["Validate"] == null)
                {
                    LoginError("验证码过期，请重新登陆！");
                    return;
                }
                string validate = CookieUtil.GetCookie("Validate");
                CookieUtil.ClearCookie("Validate");
                if (string.IsNullOrEmpty(validate) || verificationCode != validate)
                {
                    LoginError("验证码输入错误！");
                    return;
                }

                LoginRequest request = new LoginRequest();
                request.LoginId = userName;
                request.Password = password;
                request.IP = string.IsNullOrEmpty(hidIP.Value) ? "abcd" : hidIP.Value;

                var pmslogin = false;
                var pmsErr = string.Empty;
                try
                {
                    bool needChangPassword = false;
                    using (var pms = new PermissionOpenServiceClient())
                    {
                        pmslogin = pms.UserLogin(request.LoginId, request.Password, out pmsErr, out needChangPassword);
                    }

                    if (needChangPassword)
                    {
                        ToChangePassword(request.LoginId, pmsErr);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    throw new SsoException("登陆失败:" + ex.Message);
                }
                if (pmslogin == false)
                {
                    throw new SsoException("登陆失败:" + pmsErr);
                }

                SSOService sso = new SSOService();
                string token = sso.Login(request);
                if (string.IsNullOrEmpty(token))
                {
                    LoginError("登录失败");
                    return;
                }
                CookieUtil.AddCookie("ssoUcode", userName);

                string url = string.Format("{0}?ssotoken={1}&ssouid={2}",
                   GoToUrl(request.LoginId),
                   Server.UrlEncode(token),
                   Server.UrlEncode(request.LoginId));
                ValidPwdSecret(userName, password, url);
            }
            catch (SsoException ex)
            {
                LoginError(ex.Message);
            }
            catch (Exception ex)
            {
                MessageCollector.Instance.Collect(GetType(), ex.ToString(), true);
            }
        }

        /// <summary>
        /// 获得要跳到的url
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        private string GoToUrl(string loginId)
        {
            try
            {
                bool hasBackUrl = (string.IsNullOrEmpty(hidBackUrl.Value) == false);
                if (hasBackUrl)
                {
                    string backUrl = hidBackUrl.Value + GetWebAuthHandler(hidSiteId.Value);
                    return backUrl;
                }

                bool goMyDefault = (GetFormValue("GoMyDefault").ToUpper() == "ON");
                if (goMyDefault)
                {
                    string defaultSite = GetDefaultSite(loginId);

                    bool hasMyDefault = (string.IsNullOrEmpty(defaultSite) == false);
                    if (hasMyDefault)
                    {
                        return defaultSite;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageCollector.Instance.Collect(GetType(), "获得要跳到的url失败 " + ex, true);
            }

            return "WhereToGo.aspx";
        }

        /// <summary>
        /// 得到url路径
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private string GetWebAuthHandler(string siteId)
        {
            Site site = SiteList.Instance.GetSiteInfo(siteId);
            var url = new UriBuilder(site.WebAuthHandler);

            return url.Path;
        }

        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="message"></param>
        private void LoginError(string message)
        {
            ClientScript.RegisterStartupScript(GetType(), "__LoginResultInfo__", "alert(\"" + message + "\")", true);
        }

        /// <summary>
        /// 得到页面上的值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetFormValue(string name)
        {
            if (Request.Form[name] == null)
            {
                return "";
            }
            return Request.Form[name].Trim();
        }

        /// <summary>
        /// 获得默认站点
        /// </summary>
        /// <returns></returns>
        private string GetDefaultSite(string loginId)
        {
            if (string.IsNullOrEmpty(loginId))
            {
                return "";
            }

            var myDao = new MyDefaultSiteDao();
            var mysite = myDao.GetDefaultSite(loginId);

            if (string.IsNullOrEmpty(mysite) || mysite == "000")
            {
                return "";
            }

            try
            {
                Site site = SiteList.Instance.GetSiteInfo(mysite);
                return site.WebAuthHandler;
            }
            catch (Exception ex)
            {
                MessageCollector.Instance.Collect(GetType(), "获得默认站点失败" + ex.ToString(), true);
            }
            return "";

        }
        /// <summary>
        /// 验证密码复杂度 
        /// </summary>
        private void ValidPwdSecret(string uname, string val, string url)
        {
            if (GetConfigValue("ValidPwdSecret") == "N")
            {
                Response.Redirect(url, false);
            }

            if (Request.Url.Host.IndexOf("localhost") > -1)
            {
                Response.Redirect(url, false);
            }

            var rx = new Regex(@"^((?![0-9a-z]+$)(?![0-9A-Z]+$)(?![0-9\W]+$)(?![a-z\W]+$)(?![a-zA-Z]+$)(?![A-Z\W]+$)[a-zA-Z0-9\W_]).{7,19}$");
            if (rx.IsMatch(val) && !IsSimplePwd(val))
            {
                Response.Redirect(url, false);
            }
            else
            {
                ToChangePassword(uname, "");
            }
        }

        private void ToChangePassword(string uname, string msg)
        {
            if (msg.IsNullData())
            {
                msg = "你的密码过于简单，请及时修改，以免信息丢失！";
            }

            var changePwdUrl = string.IsNullOrEmpty(GetConfigValue("ChangePwdUrl")) ? "http://pms.wuliusys.com/changePwd.aspx" : GetConfigValue("ChangePwdUrl");
            var url = string.Format("{0}?usercode={1}&curl={2}", changePwdUrl, uname, Request.Url);

            Alert(msg, url);
        }

        private bool IsSimplePwd(string newPwd)
        {
            try
            {
                var secretConfigFilePath = AppDomain.CurrentDomain.BaseDirectory + "SecretRule.xml";
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(secretConfigFilePath);
                var node = xmlDoc.GetElementsByTagName("FilterValue");
                var xmlNode = node.Item(0);
                var sensitiveString = xmlNode.InnerText;

                if (sensitiveString.ToLower().IndexOf(newPwd.ToLower()) > -1)
                {
                    return true;
                }
            }
            catch { }
            return false;
        }


        /// <summary>
        /// 提示并跳转到另一个页面
        /// </summary>
        private void Alert(string message, string url)
        {
            string js = "<script language=\"javascript\">\n alert(\"" + message.Trim() +
                        "\");\n window.location.href=\""
                        + url.Trim() + "\";\n</script>";
            Page.ClientScript.RegisterStartupScript(Page.GetType(), Guid.NewGuid().ToString(), js);
        }

        private string GetConfigValue(string key)
        {
            return ConfigurationManager.AppSettings[key] == null
                 ? string.Empty
                 : ConfigurationManager.AppSettings[key].ToString();

        }
    }
}