<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TabPage.aspx.cs" Inherits="RFD.SSO.WebUI.Frame.TabPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/Styles/tabpage.css" rel="stylesheet" type="text/css" />
</head>
<body scroll="no">
    <div id="container">
        <div id="tab_menu">
        </div>
        <div id="page">
        </div>
    </div>
</body>
</html>

<script src="/Scripts/base.js" type="text/javascript"></script>

<script src="/Scripts/container.js" type="text/javascript"></script>

<script type="text/javascript">
    init();
    window.onload = window.onresize = function() {
        var pageCon = document.getElementById("page");
        var bodyHeight = document.body.clientHeight;
        pageCon.style.height = (bodyHeight - 40) + "px";
    }
</script>

