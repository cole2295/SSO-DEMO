using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RFD.SSO.Server.ServiceImpl;
using LMS.Util.Security;
using System.Web.Services;
using RFD.SSO.Server.Dao;
using LMS.Util;

namespace RFD.SSO.ServerWebHost
{
    public partial class WhereToGoV2 : System.Web.UI.Page
    {
        protected string _navi = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //if (!IsPostBack)
                //{
                string ssoUid = Request.QueryString[SsoUrlName.SSOUID];
                string ssoToken = Request.QueryString[SsoUrlName.SSOTOKEN];

                _navi = GetNavigationHtml(ssoUid, ssoToken);
                //}
            }
            catch (Exception ex)
            {
                MessageCollector.Instance.Collect(GetType(), ex.ToString(), true);
            }
        }

        protected string GetUid()
        {
            return Request.QueryString[SsoUrlName.SSOUID];
        }

        private string GetNavigationHtml(string ssoUid, string ssoToken)
        {
            //SSOService sso = new SSOService();
            //var nav = sso.GetNavigationBar();

            //foreach (var site in nav)
            //{
            //    navi += string.Format(@'<li><a target='_blank' href='{0}?{1}={2}&{3}={4}'>{5}</a></li>',
            //        site.WebAuthHandler,
            //        SsoUrlName.SSOTOKEN,
            //        HttpContext.Current.Server.UrlEncode(ClientOnlineUser.Instance.CurrentSsoToken),
            //        SsoUrlName.SSOUID,
            //        HttpContext.Current.Server.UrlEncode(ClientOnlineUser.Instance.CurrentUserId),
            //        site.SiteName);
            //}

            string naviHtml = @"<ul id='nav-shadow'>
	 		<li class='chang-one'><a class='sysLink' href='javascript:void(0)' url='http://lmsdemo.wuliusys.com/SsoAuthHandler.aspx?{0}={1}&{2}={3}' title='" + CookiesEncrypt.Encrypt("lmsdemo") + @"'>
                        <span>LMS</span></a>
                </li>
			<li class='chang-two'><a class='sysLink' href='javascript:void(0)' url='http://pmsdemo.wuliusys.com/SsoAuthHandler.aspx?{0}={1}&{2}={3}' title='" + CookiesEncrypt.Encrypt("pmsdemo") + @"'>
                        <span>PMS</span></a>
                </li>
			<li class='chang-three'><a class='sysLink' href='javascript:void(0)' url='http://fmsdemo.wuliusys.com/SsoAuthHandler.aspx?{0}={1}&{2}={3}' title='" + CookiesEncrypt.Encrypt("fmsdemo") + @"'>
                        <span>FMS</span></a>
                </li>
			<li class='chang-four'><a class='sysLink' href='javascript:void(0)' url='http://tmsdemo.wuliusys.com//Home/ssoLogin?{0}={1}&{2}={3}' title='" + CookiesEncrypt.Encrypt("tmsdemo") + @"'>
                        <span>TMS</span></a>
                </li>
			<li class='chang-five'><a class='sysLink' href='javascript:void(0)' url='http://poi.wuliusys.com/SsoAuthHandler.aspx?{0}={1}&{2}={3}' title='" + CookiesEncrypt.Encrypt("poi") + @"'>
                        <span>POI</span></a>
               </li>		
	 	</ul>";

            return string.Format(naviHtml, SsoUrlName.SSOTOKEN,
                HttpContext.Current.Server.UrlEncode(ssoToken), SsoUrlName.SSOUID,
                HttpContext.Current.Server.UrlEncode(ssoUid));
        }


        private static string GetUsercode()
        {
            if (CookieUtil.ExistCookie("ssoUcode"))
            {
                return CookieUtil.GetCookie("ssoUcode");
            }
            return "aa";
        }

        private static string MyDefaultSite(string usercode)
        {
            if (string.IsNullOrEmpty(usercode))
            {
                return "";
            }

            var myDao = new MyDefaultSiteDao();
            return myDao.GetDefaultSite(usercode);
        }

        [WebMethod]
        public static List<string> GetSites()
        {
            var usercode = GetUsercode();
            var myDefaultSite = MyDefaultSite(usercode);

            List<string> list = new List<string>() { 
                "000:请设定您的默认系统", 
                "LMS:LMS系统", 
                "TMS:TMS系统", 
                "PMS:PMS系统", 
                "FMS:FMS系统", 
                "POI:POI系统" };

            List<string> r = new List<string>();

            foreach (var v in list)
            {
                var val = v.Substring(0, 3);
                var item = v.Substring(4);
                var isMyDefaultSite = (val == myDefaultSite);

                r.Add(string.Format("<option value=\"{0}\" {1}>{2}</option>",
                    val,
                    isMyDefaultSite ? "selected=\"selected\"" : "",
                    item));
            }
            return r;
        }

        [WebMethod]
        public static string SetDefaultSite(string myDefault)
        {
            try
            {
                MyDefaultSiteDao myDao = new MyDefaultSiteDao();
                if (myDao.Merge(new Server.Model.MyDefaultSite() { DefaultSite = myDefault, UserCode = GetUsercode() }))
                {
                    return "ok";
                }
            }
            catch (Exception ex)
            {
                MessageCollector.Instance.Collect("", ex.ToString());
            }

            return "fail";
        }
    }

}