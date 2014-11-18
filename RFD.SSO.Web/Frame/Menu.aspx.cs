using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using RFD.SSO.Web;
using RFD.SSO.Web.PMSOpenService;

namespace RFD.SSO.WebUI.Frame
{
    public partial class Menu : Page
    {
        protected string JSList = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BuidMenu();
            }
        }

        /// <summary>
        /// 可变部分
        /// </summary>
        private void BuidMenu()
        {
            var client = new PermissionOpenServiceClient();
            DataTable dataTable = client.GetEmployeeMenuData("admin", "1");
            DataRow[] dataRows = dataTable.Select(" MenuLevel=0 and systemId=1");
            var id = default(int);
            for (var i = 0; i < dataRows.Length; i++)
            {
                var dataRow = dataRows[i];
                LoadMenuTitle(dataRow, i);
                DataRow[] rows = dataTable.Select(" MenuLevel=1 and MenuGroup=" + dataRow["MenuGroup"]);
                LoadMenuItem(i, rows, ref id);
                LoadMenuFoot();
            }
        }

        /// <summary>
        /// 加载菜单
        /// </summary>
        /// <param name="i"></param>
        /// <param name="rows"></param>
        /// <param name="id"></param>
        private void LoadMenuItem(int i, IEnumerable<DataRow> rows, ref int id)
        {
            foreach (var r in rows)
            {
                string sign = "&";
                if (r["URL"].ToString().IndexOf("?") == -1)
                {
                    sign = "?";
                }
                //TODO: 下面这个域名应该从数据库或枚举获取
                var url = "http://lms.wuliusys.com/" + r["URL"].ToString().Replace("..", "") + sign + "menuname=" + HttpUtility.UrlEncode(r["MenuName"].ToString());
                JSList += string.Format(@"
                <TreeNode Name='id_{2}' Text='{0}' value='{1}' Image='/Scripts/Images/16/mark.gif' href='javascript:void(0)' Target='main'></TreeNode>"
                    , r["MenuName"], url, id++);
            }
        }

        private void LoadMenuTitle(DataRow dataRow, int i)
        {
            JSList += string.Format(@"
            	<TreeNode id='ParentNode{0}' Text='{1}' Image='/Scripts/Images/vista/gif/folder.gif'>",
                i, dataRow["MenuName"]);
        }

        private void LoadMenuFoot()
        {
            JSList += "</TreeNode>";
        }
    }
}
