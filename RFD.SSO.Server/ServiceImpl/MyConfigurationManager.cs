using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace RFD.SSO.Server.ServiceImpl
{
    public class MyConfigurationManager
    {
        private static MyConfigurationManager _instance;

        private MyConfigurationManager()
        {
        }

        public static MyConfigurationManager Instance
        {
            get
            {
                return _instance ?? (_instance = new MyConfigurationManager());
            }
        }

        /// <summary>
        /// 获得配置信息
        /// </summary>
        /// <param name="configFileName">配置文件名</param>
        /// <returns></returns>
        public NameValueCollection AppSettings(string configFileName)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFileName);

                xDoc.Load(xmlPath);
            }
            catch
            {
                throw new Exception(string.Format("获取{0}配置文件失败", configFileName));
            }

            NameValueCollection nvc = new NameValueCollection();
            try
            {
                foreach (XmlNode node in xDoc.SelectNodes("configuration/appSettings/add"))
                {
                    nvc.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
                }
            }
            catch
            {
                throw new Exception(string.Format("{0}节点配置错误", configFileName));
            }

            return nvc;
        }
    }
}
