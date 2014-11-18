using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Xml;
using RFD.SSO.Server.Service;
using RFD.SSO.Server.ServiceImpl;

namespace RFD.SSO.Server.Model
{
    [Serializable]
    public class SsoToken
    {
        private SsoResponse _userData;


        /// <summary>
        /// 登录请求
        /// </summary>
        public LoginRequest LoginRequest
        {
            get;
            set;
        }
        /// <summary>
        /// 获得Token
        /// </summary>
        public string Token
        {
            get
            {
                return string.Format("{0}_{1}", LoginRequest.IP, LoginRequest.LoginId);
            }
            set
            {
                //虽然没用但在保存到mongodb里时需要set属性
            }
        }
        public DateTime LoginTime
        {
            get;
            set;
        }

        private DateTime? _validateTime;
        public DateTime? ValidateTime
        {
            get
            {
                return !_validateTime.HasValue ? LoginTime : _validateTime;
            }
            set
            {
                _validateTime = value;
            }
        }
        public bool TimeOver
        {
            get
            {
                if (_validateTime.HasValue)
                {
                    int timeOut = SiteList.Instance.TimeOut;

                    var onLineTime = (ApiDateTime.Instance.Now - _validateTime.Value).TotalMinutes;
                    if (onLineTime < 0)
                    {
                        throw new Exception("上次验证时间晚于系统当前时间");
                    }

                    return onLineTime > timeOut;
                }

                return false;
            }
        }

        /// <summary>
        /// 数据库里查出的信息
        /// </summary>
        public SsoResponse UserData
        {
            get
            {
                return _userData ?? new SsoResponse();
            }
            set
            {
                _userData = value;
            }
        }

        /// <summary>
        /// 加密后的Token
        /// </summary>
        public string EncryptedToken
        {
            get
            {
                DSA dsa = new DSA();
                return dsa.Encrypt(Token, "");
            }
        }
    }
}
