using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using RFD.SSO.WebClient;

namespace RFD.SSO.Web
{
    public class PageBase : Page
    {
        protected override void OnInit(EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (ConfigurationSettings.AppSettings["EnableHttps"] == "启用")
            {
                if (Request.Url.Scheme == "http")
                {
                    Response.Status = "301 Moved Permanently";
                    Response.AddHeader("Location", "https://lms.wuliusys.com");
                    return;
                }
            }

            if (!SsoCookieUtil.ExistCookie("RFDLMSUserID"))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "__BasePageOnInitError__", "window.open( '../Login.aspx', '_top');", true);
                return;
            }

            if (ConfigurationManager.AppSettings["ssoLogin"].ToUpper() == "Y")
            {
                SsoClient ssoClinet = new SsoClient();
                ssoClinet.ReURL();
                ProcessLogin processLogin = new ProcessLogin();
                ssoClinet.SetCurrentUser(ssoClinet.QueryUidFromUrl(), processLogin);
            }
        }
    }
}
