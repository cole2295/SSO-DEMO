<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Welcome.aspx.cs" Inherits="RFD.SSO.WebUI.Frame.Welcome" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Styles/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table style="width: 96%; margin: 20px">
        <tr>
            <td class="left_txt" colspan="2">
                当前位置： 办公桌面<hr />
            </td>
        </tr>
        <tr>
            <td align="left">
            </td>
            <td align="right" style="padding-right: 10px">
                <asp:Button ID="btnChangPassWord" runat="server" Text="修改密码" OnClick="BtnChangPassWordClick" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="notice">
                <%=noticeHtml %>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
