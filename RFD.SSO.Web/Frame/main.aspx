﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="RFD.SSO.WebUI.Frame.Main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>配送平台</title>
</head>
<frameset cols="200,8,*" id="Navigation" frameborder="0" border="0">
    <frame name="menu" src="Menu.aspx" frameborder="0" noresize="noresize">
    <frame name="menu" src="LeftLine.htm" frameborder="0" noresize="noresize">
    <frameset rows="4,*" frameborder="0" border="0">
        <frame name="header" src="TopLine.htm" frameborder="0" noresize="noresize">
        <frame name="main" src="TabPage.aspx?ssouid=<%=ssouid %>" frameborder="0" noresize="noresize">
    </frameset>
</frameset>
</html>
