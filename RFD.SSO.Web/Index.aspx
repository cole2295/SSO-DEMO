<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="RFD.SSO.Web.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<frameset name="ChangeRow" frameborder="0" framespacing="0" border="0" rows="72,*"
    scrolling="no" framespacing="0">
           <frame name="top" width="100%" scrolling="no" marginwidth="0" marginheight="0" src="Frame/Top.aspx" target="main"  noresize="noresize" />
           <frameset id="frame"cols="180,11,*">
           <frame name="left" height="100%" frameborder="NO" scrolling="auto" src="Frame/Left.aspx" noresize="noresize" target="main"/>
           <frame name="leftmenubar" height="100%" frameborder="0" scrolling="auto"  noresize="noresize" src="Frame/leftmenubar.htm" />
           <frame name="main" src="Frame/PageContainter.aspx"  marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" target="_self" />
           </frameset>
</frameset>
</html>
