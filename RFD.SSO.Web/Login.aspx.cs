using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RFD.SSO.Web
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["ssoLogin"].ToUpper() == "Y")
            {
                Response.Redirect("SsoAuthHandler.aspx", true);
                return;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("");
        }
    }
}
