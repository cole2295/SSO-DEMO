using System;
using System.Web.UI;
using RFD.SSO.Web;

namespace RFD.SSO.WebUI.Frame
{
    public partial class Welcome : Page
    {
        protected string noticeHtml = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    GetNoticeNew();
            //}
        }

        private void GetNoticeNew()
        {
            //using (var client = new PermissionOpenServiceClient())
            //{
            //    var notice = client.GetSysNotice((int)EnumCommon.SystemType.LMS_RFD_Manager, "rfd");
            //    if (notice != null) noticeHtml = notice.NoitceContent;
            //    client.Close();
            //}
        }

        protected void BtnChangPassWordClick(object sender, EventArgs e)
        {
            Response.Redirect("../BasicSetting/ChangePassword.aspx");
        }
    }
}