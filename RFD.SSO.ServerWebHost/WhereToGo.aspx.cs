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
    public partial class WhereToGo : System.Web.UI.Page
    {
        protected string _navi = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(GetUsercode()))
                {
                    Response.Redirect("SsoLogin.aspx");
                    return;
                }

                if (!IsPostBack)
                {
                    string ssoUid = Request.QueryString[SsoUrlName.SSOUID];
                    string ssoToken = Request.QueryString[SsoUrlName.SSOTOKEN];

                    _navi = GetNavigationHtml(ssoUid, ssoToken);
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
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
            string naviHtml = "<div class='clearfix masonry' id='container' style='position: relative; height: 1320px;'>" + GetNavHtml() + "</div>";

            return string.Format(naviHtml, SsoUrlName.SSOTOKEN,
                HttpContext.Current.Server.UrlEncode(ssoToken), SsoUrlName.SSOUID,
                HttpContext.Current.Server.UrlEncode(ssoUid));
        }

        private string GetNavHtml()
        {
            string html = "";

            foreach (var s in SiteList.Instance.UsableSites)
            {
                html += string.Format(@"<div style='position: absolute; top: 5px; left: 605px;' class='box photo col2 masonry-brick'>
                    <a class='sysLink' href='javascript:void(0)' url='{0}' title='{2}'>
                        <img src='SsoStyles/images/{1}' alt='{2}'></a>
                </div>", s.WebAuthHandler + "?{0}={1}&{2}={3}", s.Pic, s.SiteName);
            }

            return html;
        }

        private static string GetUsercode()
        {
            if (CookieUtil.ExistCookie("ssoUcode"))
            {
                return CookieUtil.GetCookie("ssoUcode");
            }
            return "";
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

            List<string> r = new List<string>();
            r.Add("<option value=\"000\">请设定您的默认系统</option>");

            foreach (var s in SiteList.Instance.UsableSites)
            {
                var isMyDefaultSite = (s.SiteCode.ToLower() == myDefaultSite.ToLower());

                r.Add(string.Format("<option value=\"{0}\" {1}>{2}</option>",
                    s.SiteCode.ToUpper(),
                    isMyDefaultSite ? "selected=\"selected\"" : "",
                    s.SiteName));
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

    public class SsoUrlName
    {
        public const string SSOTOKEN = "ssotoken";
        public const string SSOUID = "ssouid";
    }
}