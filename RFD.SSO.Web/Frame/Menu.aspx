<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="RFD.SSO.WebUI.Frame.Menu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Styles/frame.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/base.js" type="text/javascript"></script>
    <script src="../Scripts/container.js?v=1" type="text/javascript"></script>
</head>
<body>
    <div style="width: 100%; height: 100%; overflow-x: hidden;">
        <treeview id="TreeView1" theme="vista" style="padding: 5px 0px 5px 5px;" width="100%"
            height="100%" backcolor="#FFFFFF" borderwidth="0px" borderstyle="solid" bordercolor="#000000"
            nodeindent="16" nodespacing="0" nodepadding="2" childnodespadding="0" showimages="true"
            showcheckboxes="false" showdebugnode="false" expandonselect="true" selectedpath=""
            checkedpaths="" expanddepth="1" xml="">
				<%=JSList %>
		</treeview>
    </div>
    <script type="text/javascript" src="../Scripts/Root.TreeView.js?v=1"></script>
</body>
</html>
