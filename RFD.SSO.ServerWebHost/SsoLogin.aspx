<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SsoLogin.aspx.cs" Inherits="RFD.SSO.ServerWebHost.SsoLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script src="SsoScripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<title>风云配送平台</title>
<style type="text/css">
html,body{height:100%; width:100%; overflow:hidden; margin:0; padding:0; background:#80badf url(SsoStyles/images/login_bg.jpg) top repeat-x; font-size:12px;}
#container{height:100%; width:100%; position:relative;}
#login_main{background:url(SsoStyles/images/login_main.jpg); width:659px; height:554px; margin:0 auto; position:relative;}
.login_input{border:0; background-color:transparent; line-height:22px; height:22px;position:absolute; z-index:2;}
.login_button{background:url(SsoStyles/images/btnLogin.gif); width:106px; height:35px; margin:0; padding:0;}
#imgVerificationCode{position:absolute;	z-index:2;vertical-align:middle;cursor:pointer;}
#GoMyDefault{position:absolute;	z-index:2; color:#1979a9; font-size:14px;}
#GoMyDefault input{ display:none;}
#GoMyDefault img{ background:url(SsoStyles/images/checkbox.png) top; height:12px; width:12px; margin-bottom:-1px;}
#copyright{position:relative; top:550px; text-align:center; color:#fff; line-height:22px;}
</style>
<script type="text/javascript">
    function setMyDefault() {
        var imgCheck = document.getElementById("imgCheck");
        var chkGoMyDefault = document.getElementById("chkGoMyDefault");
        chkGoMyDefault.checked = !chkGoMyDefault.checked;
        imgCheck.style.backgroundPosition = chkGoMyDefault.checked ? "bottom" : "top";
    }
    $(document).ready(function () {
        setMyDefault();
        $(".login_button").bind("click", function (event) {
            if ($.trim($("#txtUserName").val()) == "") { alert("请输入用户名"); event.preventDefault(); return; }
            if ($.trim($("#txtPassword").val()) == "") { alert("请输入密码"); event.preventDefault(); return; }
            if ($.trim($("#txtVerificationCode").val()) == "") { alert("请输入验证码"); event.preventDefault(); return; }
        });
        $("#imgVerificationCode").attr("src", "ssologinpage/Validate.aspx"+'?'+Math.random());
    });

</script>
</head>

<body>
<div id="container">
    <div id="login_main">
      <form id="Form1" runat="server" autocomplete="off">
        <input runat="server" type="text" id="txtUserName" name="UserName" class="login_input" style="top:286px; left:405px; width: 175px;"/>
        <input type="password" id="txtPassword" name="Password"  class="login_input" style="top:326px; left:405px; width: 175px;" />
        <input type="text" id="txtVerificationCode" name="VerificationCode" 
            class="login_input" style="top:366px; left:405px; width: 88px;" maxlength="4"  />
        <img id="imgVerificationCode" title="点击更换验证码"  onclick="this.src=this.src+'?'+Math.random()"  style="top:366px;left:507px;" />		
        <asp:Button ID="btnLogin" runat="server" class="login_input login_button" 
            style="top:443px; left:399px;" value="" onclick="btnLogin_Click"/>
        <span id="GoMyDefault" style="top:410px; left:398px;" onclick="setMyDefault();">
        <img id="imgCheck" src="SsoStyles/images/blank.gif" />
        <input type="checkbox" id="chkGoMyDefault" name="GoMyDefault"  />
        <label>进入我的默认</label></span>   
        <asp:HiddenField ID="hidIP" runat="server" />
        <asp:HiddenField ID="hidSiteId" runat="server" />
        <asp:HiddenField ID="hidBackUrl" runat="server" />
      </form>
      <div id="copyright">版权所有 Copyright 2011-2015  All Rights Reserved
      <br />
      建议使用1024*768以上分别率, IE7以上版本浏览器访问该系统！
      </div>
    </div>
    </div>
</body>
</html>
