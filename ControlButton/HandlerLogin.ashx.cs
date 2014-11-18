using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using RFD.Message;

namespace ControlButton
{
    /// <summary>
    /// Summary description for HandlerLogin
    /// </summary>
    public class HandlerLogin : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string callback = string.Empty;
            SiteHost siteHost = GetSiteHost("default");

            try
            {
                callback = context.Request.QueryString.Get("jsoncallback");

                string host = string.Empty;
                if (context.Request.UrlReferrer != null)
                {
                    host = context.Request.UrlReferrer.Host.ToLower();
                }
                MessageCollector.Instance.Collect("", string.Format("host:{0}", host));

                siteHost = SiteList.FirstOrDefault(p => p.Host == host) ?? GetSiteHost("default");
            }
            catch (Exception ex)
            {
                MessageCollector.Instance.Collect("", ex.ToString(), true);
            }

            string site = (siteHost.Host.Contains("demo") || siteHost.Host.Contains("default")) ?
                "clouddemo.wuliusys.com" : "cloud.wuliusys.com";
            string tips = siteHost.Enabled ? "小提示：可以通过云平台{0}登录，稍后此登录界面将会停用，谢谢" : "小提示:请使用云平台{0}登录，谢谢";

            site = string.Format("<a style='font-size: 15pt; color:Blue; ' href='http://{0}'>{0}</a>", site);
            tips = string.Format(tips, site);
            //tips = string.Format(tips, "<span id='tipsite' style='font-size: 14pt;'>" + site + "</span>");
            tips = siteHost.IsTip ? tips : "";

            string logginBtn = (siteHost.Enabled ? "enabled" : "disabled");

            var res = "{\"LoginButton\":\"" + logginBtn + "\",\"Tips\":\"" + tips + "\"}";

            context.Response.Clear();
            context.Response.ContentType = "text/plain";
            context.Response.Write(callback + "(" + res + ")");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private static List<SiteHost> SiteList
        {
            get
            {
                List<SiteHost> sysList = new List<SiteHost>();
                sysList.Add(GetSiteHost("lms.wuliusys.com"));
                sysList.Add(GetSiteHost("pms.wuliusys.com"));
                sysList.Add(GetSiteHost("tms.wuliusys.com"));
                sysList.Add(GetSiteHost("fms.wuliusys.com"));
                sysList.Add(GetSiteHost("poi.wuliusys.com"));
                sysList.Add(GetSiteHost("crm.wuliusys.com"));
                sysList.Add(GetSiteHost("lmsdemo.wuliusys.com"));
                sysList.Add(GetSiteHost("pmsdemo.wuliusys.com"));
                sysList.Add(GetSiteHost("tmsdemo.wuliusys.com"));
                sysList.Add(GetSiteHost("fmsdemo.wuliusys.com"));
                sysList.Add(GetSiteHost("poidemo.wuliusys.com"));
                sysList.Add(GetSiteHost("crmdemo.wuliusys.com"));
                return sysList;
            }
        }

        private static SiteHost GetSiteHost(string confName)
        {
            if (ConfigurationManager.AppSettings[confName] == null)
            {
                return new SiteHost { Host = confName, Enabled = true, IsTip = false };
            }

            string confValue = ConfigurationManager.AppSettings[confName].ToUpper();

            return new SiteHost
            {
                Host = confName,
                Enabled = (confValue.Substring(0, 1) == "Y"),
                IsTip = (confValue.Substring(1, 1) == "Y")
            };
        }
    }

    public class SiteHost
    {
        public string Host { get; set; }
        public bool Enabled { get; set; }
        public bool IsTip { get; set; }
    }
}