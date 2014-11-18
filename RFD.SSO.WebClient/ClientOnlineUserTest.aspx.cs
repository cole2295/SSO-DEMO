using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RFD.SSO.WebClient
{
    public partial class ClientOnlineUserTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            SsoClient ssoClient = new SsoClient();
           var  _navi = ssoClient.GetNavigationHtml();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ClientOnlineUser.Instance.Add(TextBox1.Text, TextBox2.Text);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            ClientOnlineUser.Instance.GetSsoToken(TextBox1.Text);
        }
    }
}
