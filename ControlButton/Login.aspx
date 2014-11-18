<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ControlButton.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $.getJSON("HandlerLogin.ashx?jsoncallback=?", function (json) {
            if ('disabled' == json.LoginButton) {
                $('input').attr('disabled', 'disabled');
            } else {
                $('input').removeAttr('disabled');
            };
            $('#tip').html(json.Tips);
        });
        
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:button id="Button1" runat="server" text="Button" enabled="False" />
        <div>
            <span id="tip">小提示：云系统demo已经上线，可以尝试通过云平台登录， 稍后此登录界面将会停用<br />
                请多通过云平台登录，谢谢 <a href="http://clouddemo.wuliusys.com">clouddemo.wuliusys.com</a></span></div>
    </div>
    <div id="images">
    </div>
    </form>
</body>
</html>
<script type="text/javascript">
    document.getElementById("Button1").disabled = false
</script>
