using System;
using System.Web.UI;

namespace RFD.SSO.WebUI.Frame
{
    public partial class Main : Page
    {
        protected string ssouid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            //ssouid = GetQueryString("ssouid");
        }
        protected string GetQueryString(string key)
        {
            return Request.QueryString[key] == null ? string.Empty : Request.QueryString[key].ToString();
        }
    }
}