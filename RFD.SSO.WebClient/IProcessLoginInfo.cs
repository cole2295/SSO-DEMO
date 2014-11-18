using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RFD.SSO.WebClient
{
    /// <summary>
    /// 处理登录成功返回的数据
    /// </summary>
    public interface IProcessLoginInfo
    {
        void ProcessLoginInfo(SsoResponse ssoResponse);
    }
}
