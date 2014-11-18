<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientOnlineUserTest.aspx.cs"
    Inherits="RFD.SSO.WebClient.ClientOnlineUserTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
    </div>
    <div>
        <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="Button" />
    </div>
    </form>
</body>
</html>
