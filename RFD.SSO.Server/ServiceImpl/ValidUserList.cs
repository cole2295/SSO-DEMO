using System;
using System.Collections.Generic;
using System.Linq;
using LMS.Util;
using System.Data;
using RFD.SSO.Server.Dao;
using RFD.SSO.Server.Domain;
using RFD.SSO.Server.Model;
using RFD.SSO.Server.Service;

namespace RFD.SSO.Server.ServiceImpl
{
    public class ValidUserList
    {
        private IUserDao _userDod = null;
        private static ValidUserList _instance = null;

        private ValidUserList()
        {
            UserDao = new UserDao();
            DB = new OnlineUserDao();//MongoDb();
            OnlineUserLog = new MongoDb();
        }

        public static ValidUserList Instance
        {
            get
            {
                return _instance ?? (_instance = new ValidUserList());
            }
        }

        /// <summary>
        /// 登录并获得凭证
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns>返回凭证</returns>
        public string Login(LoginRequest loginRequest)
        {
            if (null == loginRequest)
            {
                return "";
            }

            if (string.IsNullOrEmpty(loginRequest.IP))
            {
                throw new Exception("IP为空");
            }

            Employee employee = new Employee
            {
                EmployeeCode = loginRequest.LoginId,
                PassWord = loginRequest.Password
            };

            var userData = GetUserData(employee);
            if (userData == null || userData.EmployeeCode.ToUpper() != loginRequest.LoginId.ToUpper())
            {
                return "";
            }

            SsoToken ssoToken = new SsoToken
                                    {
                                        LoginRequest = loginRequest,
                                        LoginTime = ApiDateTime.Instance.Now,
                                        UserData = userData
                                    };

            if (DB.Add(ssoToken))
            {
                LoginLog(ssoToken);

                //返回加密后凭证
                return ssoToken.EncryptedToken;
            }

            return "";
        }

        /// <summary>
        /// 把登录信息记录到数据库里
        /// </summary>
        /// <param name="ssoToken"></param>
        private void LoginLog(SsoToken ssoToken)
        {
            try
            {
                if (null != OnlineUserLog)
                {
                    OnlineUserLog.Add(ssoToken);
                }
            }
            catch (Exception ex)
            {
                MessageCollector.Instance.Collect("记录登录日志失败", ex.ToString(), true);
            }
        }

        public void Logout(string siteId, string token)
        {
            string realToken = DecryptToken(siteId, token);

            DB.Remove(realToken);
        }

        /// <summary>
        /// 验证凭证并获得用户信息
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="token"></param>
        /// <param name="ssoResponse">用户信息</param>
        /// <returns></returns>
        public bool ValidateToken(string siteId, string ip, string token, out SsoResponse ssoResponse)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("登录凭证token为空");
            }

            if (string.IsNullOrEmpty(ip))
            {
                throw new Exception("IP为空");
            }

            string realToken = DecryptToken(siteId, token);
            //验证是同一个浏览器客户端
            if (realToken.Substring(0, ip.Length) != ip)
            {
                ssoResponse = null;
                return false;
            }

            SsoToken ssoToken = GetOne(realToken);
            //没有token
            if (null == ssoToken)
            {
                ssoResponse = null;
                return false;
            }

            //token超时
            if (ssoToken.TimeOver)
            {
                ssoResponse = null;
                DB.Remove(ssoToken.Token);
                return false;
            }

            //更新Tokens
            ssoToken.ValidateTime = ApiDateTime.Instance.Now;

            try
            {
                DB.Update(ssoToken);
            }
            catch (Exception ex)
            {
                throw new Exception("访问文件失败： " + ex.ToString());
            }

            ssoResponse = ssoToken.UserData;

            return ssoResponse != null;
        }

        private string DecryptToken(string siteId, string token)
        {
            DSA dsa = new DSA();
            Site site = SiteList.Instance.GetSiteInfo(siteId);

            if (site == null)
            {
                throw new Exception("未注册的web站点");
            }

            return dsa.Decrypt(token, site.PublicKey);
        }

        public SsoResponse GetUserData(Employee employee)
        {
            DataTable userData = _userDod.UserLogIn(employee);

            if (userData == null || userData.Rows.Count <= 0)
            {
                return null;
            }

            if (userData.Rows[0]["EmployeeCode"] == null ||
                string.IsNullOrEmpty(userData.Rows[0]["EmployeeCode"].ToString()))
            {
                return null;
            }

            return new SsoResponse
            {
                Companyname = userData.Rows[0]["companyname"].ToString(),
                DistributionCode = userData.Rows[0]["DistributionCode"].ToString(),
                EmployeeCode = userData.Rows[0]["EmployeeCode"].ToString(),
                EmployeeID = userData.Rows[0]["EmployeeID"].ToString().TryGetInt(),
                EmployeeName = userData.Rows[0]["EmployeeName"].ToString(),
                StationID = userData.Rows[0]["StationID"].ToString().TryGetInt(),
                SysManager = userData.Rows[0]["SysManager"].ToString().TryGetInt()
            };
        }

        public virtual SsoToken GetOne(string token)
        {
            return DB.GetOne(token);
        }

        public IDB DB
        {
            get;
            set;
        }

        public IDB OnlineUserLog
        {
            get;
            set;
        }

        public IUserDao UserDao
        {
            set
            {
                _userDod = value;
            }
        }

    }
}
