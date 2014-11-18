using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace RFD.SSO.WebClient
{
    public partial class index : System.Web.UI.Page
    {
        protected string _navi = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            SsoClient ssoClient = new SsoClient();
            _navi = ssoClient.GetNavigationHtml();

        }
    }
}
