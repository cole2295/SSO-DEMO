using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace RFD.SSO.WebClient
{
    public class SsoCookieUtil
    {
        /// <summary>
        /// 添加Cookie
        /// </summary>
        /// <param name="CookieName">Cookie名称</param>
        /// <param name="CookieValue">Cookie值</param>
        /// <param name="ExpiresTime">有效时间</param>
        public static void AddCookie(string CookieName, string CookieValue, int ExpiresTime)
        {
            CookieValue = HttpContext.Current.Server.UrlEncode(CookieValue);
            if (HttpContext.Current.Request.Cookies[CookieName] != null)
            {
                ClearCookie(CookieName);//如果存在则清空
            }

            var MyCookie = new HttpCookie(CookieName)
            {
                Value = SsoCookiesEncrypt.Encrypt(CookieValue),
                Expires = DateTime.Now.AddHours(ExpiresTime),
                //#if !Debug
                //Domain = ".wuliusys.com"
                //#endif
            };

            HttpContext.Current.Response.Cookies.Add(MyCookie);
        }

        public static void AddCookie(string CookieName, string CookieValue, int ExpiresTime, string domain)
        {
            CookieValue = HttpContext.Current.Server.UrlEncode(CookieValue);
            if (HttpContext.Current.Request.Cookies[CookieName] != null)
            {
                ClearCookie(CookieName);//如果存在则清空
            }

            var MyCookie = new HttpCookie(CookieName)
            {
                Value = SsoCookiesEncrypt.Encrypt(CookieValue),
                Expires = DateTime.Now.AddHours(ExpiresTime),

                Domain = domain

            };

            HttpContext.Current.Response.Cookies.Add(MyCookie);
        }


        /// <summary>
        /// 根据Cookie名称获取Cookie的值
        /// </summary>
        /// <param name="CookieName"></param>
        public static string GetCookie(string CookieName)
        {
            try
            {
                return HttpContext.Current.Server.UrlDecode(SsoCookiesEncrypt.Decrypt(HttpContext.Current.Request.Cookies[CookieName].Value));
            }
            catch (Exception ex)
            {
                throw new Exception("未找到该cookie值！" + CookieName + DateTime.Now + ex.Message + ex.Source + ex.StackTrace);
            }
        }

        /// <summary>
        /// 清理Cookie
        /// </summary>
        public static void ClearCookie(string CookieName)
        {
            try
            {
                if (HttpContext.Current.Request.Cookies[CookieName] != null)
                {
                    HttpCookie mycookie = HttpContext.Current.Request.Cookies[CookieName];
                    var ts = new TimeSpan(0, 0, 0, 0); //时间跨度 
                    mycookie.Expires = DateTime.Now.Add(ts); //立即过期 
                    HttpContext.Current.Response.Cookies.Remove(CookieName); //清除 
                    HttpContext.Current.Response.Cookies.Add(mycookie); //写入立即过期的*/
                    HttpContext.Current.Response.Cookies[CookieName].Expires = DateTime.Now.AddDays(-1);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("操作cookie！" + CookieName + DateTime.Now + ex.Message + ex.Source + ex.StackTrace);
            }
        }

        public static bool ExistCookie(string cookieName)
        {
            return HttpContext.Current.Request.Cookies.AllKeys.ToList().Exists(item => item == cookieName);
        }

        public static void AddCookie<T>(string name, T value)
        {
            ClearCookie(name);
            AddCookie(name, value.ToString(), 1);
        }

        public static void AddCookie<T>(string name, T value, int expiresTime)
        {
            ClearCookie(name);

            AddCookie(name, value.ToString(), expiresTime);
        }

        //public static T GetCookie<T>(string name)
        //{
        //    try
        //    {
        //        return DataConvert.ToValue<T>(GetCookie(name));
        //    }
        //    catch (Exception ex)
        //    {
        //        return default(T);
        //    }
        //}
        public static string GetLastCookie(HttpCookie cookies)
        {
            try
            {
                return HttpContext.Current.Server.UrlDecode(SsoCookiesEncrypt.Decrypt(cookies.Value));
            }
            catch (Exception ex)
            {
                throw new Exception("未找到该cookie值！" + cookies.Name + DateTime.Now + ex.Message + ex.Source + ex.StackTrace);
            }
        }

        /// <summary>
        /// 获取客户端信息已经服务的错误信息
        /// </summary>
        /// <returns></returns>
        public static string GetHttpRequestAndError(Exception exception)
        {
            StringBuilder RequestAndErrorMsg = new StringBuilder();
            if (HttpContext.Current.Request.Cookies["RFDLMSUserID"] != null)
            {
                RequestAndErrorMsg.Append("访问时间：" + DateTime.Now + "<br>");
                RequestAndErrorMsg.Append("员工ID：" + SsoCookieUtil.GetCookie("RFDLMSUserID") + "<br>");
                RequestAndErrorMsg.Append("员工名称：" + SsoCookieUtil.GetCookie("RFDLMSUserName") + "<br>");
                RequestAndErrorMsg.Append("员工代号：" + SsoCookieUtil.GetCookie("RFDLMSUserCode") + "<br>");
                RequestAndErrorMsg.Append("所在部门ID：" + Convert.ToInt32(SsoCookieUtil.GetCookie("RFDLMSExpressID")) + "<br>");
                RequestAndErrorMsg.Append("所在部门名称：" + SsoCookieUtil.GetCookie("RFDLMSExpressName") + "<br>");
                RequestAndErrorMsg.Append("配送商编号：" + SsoCookieUtil.GetCookie("DistributionCode") + "<br>");
            }
            string user_IP = "";
            //HttpContext.Current.Request.
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                user_IP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else
            {
                user_IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }

            RequestAndErrorMsg.Append("客户端IP：" + user_IP + "<br>");
            RequestAndErrorMsg.Append("客户端DNS主机名：" + HttpContext.Current.Request.UserHostName + "<br>");
            RequestAndErrorMsg.Append("客户端使用平台：" + HttpContext.Current.Request.Browser.Platform + "<br>");
            RequestAndErrorMsg.Append("客户端使用浏览器：" + HttpContext.Current.Request.Browser.Type + "<br>");
            RequestAndErrorMsg.Append("客户端浏览器版本号：" + HttpContext.Current.Request.Browser.Version + "<br>");
            RequestAndErrorMsg.Append("客户端请求URL：" + HttpContext.Current.Request.Url + "<br>");
            RequestAndErrorMsg.Append(string.Format("错误页面：{0}\r\nMessage:{1}\r\nSource:{2}\r\nTrace:{3}",
                                                    HttpContext.Current.Request.Url, exception.Message, exception.Source, exception.StackTrace));
            return RequestAndErrorMsg.ToString();
        }

        /// <summary>
        /// 获取客户端信息已经服务的错误信息
        /// </summary>
        /// <returns></returns>
        public static string GetHttpRequestForLogin(Exception exception)
        {
            StringBuilder RequestAndErrorMsg = new StringBuilder();

            string user_IP = "";
            //HttpContext.Current.Request.
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                user_IP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else
            {
                user_IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }

            RequestAndErrorMsg.Append("客户端IP：" + user_IP + "<br>");
            RequestAndErrorMsg.Append("客户端DNS主机名：" + HttpContext.Current.Request.UserHostName + "<br>");
            RequestAndErrorMsg.Append("客户端使用平台：" + HttpContext.Current.Request.Browser.Platform + "<br>");
            RequestAndErrorMsg.Append("客户端使用浏览器：" + HttpContext.Current.Request.Browser.Type + "<br>");
            RequestAndErrorMsg.Append("客户端浏览器版本号：" + HttpContext.Current.Request.Browser.Version + "<br>");
            return RequestAndErrorMsg.ToString();
        }
    }

    /// <summary> 
    /// Cookies加密/解密
    /// Create By Liuxiaogang
    /// </summary> 
    public class SsoCookiesEncrypt
    {
        /// <summary>
        /// 当前程序加密所使用的密钥8位
        /// </summary>
        private const string DES_KEY = "FD!@7*$)";

        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="pToEncrypt">需要加密字符串</param>
        /// <param name="sKey">密钥</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(string pToEncrypt)
        {
            return Encrypt(pToEncrypt, Encoding.UTF8);
        }

        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="pToEncrypt">需要加密字符串</param>
        /// <param name="sKey">密钥</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(string pToEncrypt, Encoding encoding)
        {
            try
            {
                var des = new DESCryptoServiceProvider();
                byte[] inputByteArray = encoding.GetBytes(pToEncrypt);
                //建立加密对象的密钥和偏移量
                des.Key = ASCIIEncoding.ASCII.GetBytes(DES_KEY);
                des.IV = ASCIIEncoding.ASCII.GetBytes(DES_KEY);

                ICryptoTransform desencrypt = des.CreateEncryptor();
                byte[] result = desencrypt.TransformFinalBlock(inputByteArray, 0, inputByteArray.Length);
                return BitConverter.ToString(result);
            }
            catch (Exception ex)
            {
                throw (new Exception("Invalid Key or input string is not a valid string", ex));
            }
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="pToDecrypt">需要解密的字符串</param>
        /// <param name="sKey">密匙</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypt(string pToDecrypt)
        {
            return Decrypt(pToDecrypt, Encoding.UTF8);
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="pToDecrypt">需要解密的字符串</param>
        /// <param name="sKey">密匙</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypt(string pToDecrypt, Encoding encoding)
        {
            try
            {
                var DES = new DESCryptoServiceProvider();
                string[] sInput = pToDecrypt.Split("- ".ToCharArray());
                byte[] data = new byte[sInput.Length];
                for (int i = 0; i < sInput.Length; i++)
                {
                    data[i] = byte.Parse(sInput[i], NumberStyles.HexNumber);
                }
                DES.Key = ASCIIEncoding.ASCII.GetBytes(DES_KEY);
                DES.IV = ASCIIEncoding.ASCII.GetBytes(DES_KEY);
                ICryptoTransform desencrypt = DES.CreateDecryptor();
                byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                return encoding.GetString(result);
            }
            catch (Exception ex)
            {
                throw (new Exception("Invalid Key or input string is not a valid string", ex));
            }
        }
    }
}