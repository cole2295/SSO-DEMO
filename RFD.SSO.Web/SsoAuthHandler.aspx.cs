using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using LMS.Util;
using RFD.SSO.WebClient;

namespace RFD.SSO.Web
{
    public partial class SsoAuthHandler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SsoClient ssoClinet = new SsoClient();
                ProcessLogin processLogin = new ProcessLogin();

                if (!ssoClinet.Login(ssoClinet.QueryUidFromUrl(),
                    ssoClinet.QueryTokenFromUrl(),
                    processLogin))
                {
                    ssoClinet.RedirectToLoginPage();
                }
                Response.Redirect(string.Format("index.aspx?ssotoken={0}&ssouid={1}",
                       HttpContext.Current.Request.QueryString["ssotoken"],
                       HttpContext.Current.Request.QueryString["ssouid"]), true);
            }
        }
    }

    public class ProcessLogin : IProcessLoginInfo
    {
        public void ProcessLoginInfo(SsoResponse loginInfo)
        {
            var userid = loginInfo.EmployeeID.ToString();
            var userCode = loginInfo.EmployeeCode;
            var userName = loginInfo.EmployeeName;
            var expressId = loginInfo.StationID;
            var expressName = loginInfo.Companyname;
            var distributionCode = loginInfo.DistributionCode;
            var sysManager = loginInfo.SysManager.ToString();

            SsoCookieUtil.AddCookie("RFDLMSUserID", userid, 18);
            SsoCookieUtil.AddCookie("RFDLMSUserCode", userCode, 18);
            SsoCookieUtil.AddCookie("RFDLMSUserName", userName, 18);
            SsoCookieUtil.AddCookie("RFDLMSExpressID", expressId, 18);
            SsoCookieUtil.AddCookie("RFDLMSExpressName", expressName, 18);
            SsoCookieUtil.AddCookie("DistributionCode", distributionCode, 18);
            SsoCookieUtil.AddCookie("SysManager", sysManager, 18);

            //var authentication = ServiceLocator.GetService<IRFDCustomAuthentication>();
            //authentication.InitAuthenticationMulti();
        }
    }
}
