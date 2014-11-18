<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WhereToGo.aspx.cs" Inherits="RFD.SSO.ServerWebHost.WhereToGo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="SsoStyles/style.css" />
    <script src="/SsoScripts/jquery-1.7.2.min.js"></script>
    <script src="SsoScripts/jquery.cookie.js" type="text/javascript"></script>
    <script src="/SsoScripts/jquery.masonry.min.js"></script>
    <script type="text/javascript">
        $(function () {

            var $container = $('#container');

            $container.imagesLoaded(function () {
                $container.masonry({
                    itemSelector: '.box'
                });
            });

        });
        $(document).ready(function () {
            $('.sysLink').bind('click', function (event) {
                //event.preventDefault();   
                $.cookie('sysId<%=GetUid() %>', $(this).attr('title'));
                window.location = $(this).attr('url');
            });

            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "WhereToGo.aspx/GetSites",
                data: null,
                dataType: "json",
                success: function (result) {
                    var obj = result.d;
                    $.each(obj, function (i, item) {
                        $("#setDefaultSite").append(item);
                    });
                    $("#setDefaultSite").show();
                },
                error: function () { alert("程序异常"); }
            });

            var currentValue = $("#setDefaultSite").val();
            $("#setDefaultSite").change(function () {
                if (currentValue != $("#setDefaultSite").val()) {
                    currentValue = $("#setDefaultSite").val();
                    $.ajax({
                        type: "POST",
                        contentType: "application/json",
                        url: "WhereToGo.aspx/SetDefaultSite",
                        data: "{myDefault:'" + currentValue + "'}",
                        dataType: "json",
                        success: function (result) {
                            if (result.d == 'ok') {
                                alert("设定成功，你的默认站点修改为："
                                    + $("#setDefaultSite").find('option:selected').text());
                            } else {
                                alert("设定默认系统失败！");
                            }
                        },
                        error: function () { alert("程序异常,"); }
                    });
                }
            });
        });
    </script>
    <style type="text/css">
        .item
        {
            width: 220px;
            margin: 10px;
            float: left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="width: 700px; margin-left: auto; margin-right: auto;">
            <%=_navi%>
        </div>
    </div>
    <div>
        <div style="width: 700px; margin-left: auto; margin-right: auto;">
            <span style="font-size: 14pt;">设定我的默认</span>
            <select id="setDefaultSite">
            </select>
        </div>
    </div>
    </form>
</body>
</html>
