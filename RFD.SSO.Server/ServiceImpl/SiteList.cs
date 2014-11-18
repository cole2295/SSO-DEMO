using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using RFD.SSO.Server.Model;

namespace RFD.SSO.Server.ServiceImpl
{
    public class SiteList
    {
        private static SiteList _instance = null;
        //private readonly static SiteList _instance;

        private List<Site> _siteList = null;
        private int _timeOut;
        private string _loginUrl = string.Empty;

        private SiteList()
        {
            _siteList = GetSiteFromXml();
            _loginUrl = GetLoginUrlFromXml();
            _timeOut = GetTimeOutFromXml();
        }

        //static SiteList()
        //{
        //    _instance = new SiteList();
        //}


        public static SiteList Instance
        {
            get { return _instance ?? (_instance = new SiteList()); }
        }

        private List<Site> GetSiteFromXml()
        {
            XmlDocument xDoc = new XmlDocument();

            try
            {
                var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sites.xml");

                xDoc.Load(xmlPath);
            }
            catch
            {
                throw new Exception("获取Sites.xml配置文件失败");
            }


            List<Site> siteList = new List<Site>();
            try
            {
                foreach (XmlNode node in xDoc.SelectNodes("sites/site"))
                {
                    Site site = new Site();
                    site.SiteId = node.Attributes["SiteId"].Value;
                    site.SiteCode = node.Attributes["SiteCode"].Value;
                    site.Pic = node.Attributes["Pic"].Value;
                    site.Usable = node.Attributes["Usable"].Value;
                    site.PublicKey = node.Attributes["PublicKey"].Value;
                    site.PublicKeyMode = node.Attributes["PublicKeyMode"].Value;
                    site.WebAuthHandler = node.Attributes["WebAuthHandler"].Value;
                    site.SiteName = node.Attributes["SiteName"].Value;

                    siteList.Add(site);
                }
            }
            catch
            {
                throw new Exception("Sites.xml节点配置错误");
            }

            return siteList;
        }

        private string GetLoginUrlFromXml()
        {
            string loginUrl;
            XmlDocument xDoc = new XmlDocument();

            try
            {
                var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sites.xml");
                xDoc.Load(xmlPath);
                loginUrl = xDoc.SelectNodes("sites")[0].Attributes["loginUrl"].Value;
            }
            catch
            {
                throw new Exception("获取Sites.xml配置文件失败");
            }
            return loginUrl;
        }

        private int GetTimeOutFromXml()
        {
            int timeOut;
            XmlDocument xDoc = new XmlDocument();

            try
            {
                var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sites.xml");
                xDoc.Load(xmlPath);
                timeOut = int.Parse(xDoc.SelectNodes("sites")[0].Attributes["timeout"].Value);
            }
            catch
            {
                throw new Exception("获取Sites.xml配置文件失败");
            }
            return timeOut;
        }

        public Site GetSiteInfo(string siteId)
        {
            Site site = null;
            if (_siteList != null)
            {
                site = _siteList.FirstOrDefault(s => s.SiteId.ToLower().Contains(siteId.ToLower()));
            }

            if (site == null)
            {
                throw new Exception(string.Format("未经验证的站点{0}", siteId));
            }

            return site;
        }

        public List<Site> Sites
        {
            get { return _siteList; }
        }

        public List<Site> UsableSites
        {
            get
            {
                if (null == _siteList) { return null; }

                return _siteList.Where(w => w.Usable.ToUpper() == "Y").ToList();
            }
        }

        public int TimeOut
        {
            get
            {
                if (_timeOut <= 0)
                {
                    throw new Exception("请确认Sites.xml配置文件里设定timeout了timeout属性,如<sites timeout='30'>");
                }
                return _timeOut;
            }
        }

        public string LoginUrl
        {
            get { return _loginUrl; }
        }
    }
}
