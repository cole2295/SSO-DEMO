using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LMS.Util;

namespace RFD.SSO.WebClient
{
    public partial class _Default : System.Web.UI.Page, IProcessLoginInfo
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SsoClient ssoClient = new SsoClient();

                ssoClient.Login(this);

                Response.Redirect(string.Format("index.aspx?ssotoken={0}&ssouid={1}",
                    CookieUtil.GetCookie(SsoCookieName.SSO_TOKEN),
                    CookieUtil.GetCookie(SsoCookieName.SSO_UID)), true);
            }
        }

        public void ProcessLoginInfo(DataTable loginInfo)
        {

        }
    }
}
