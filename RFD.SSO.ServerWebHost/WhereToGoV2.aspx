<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WhereToGoV2.aspx.cs" Inherits="RFD.SSO.ServerWebHost.WhereToGoV2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="/SsoScripts/jquery-1.7.2.min.js"></script>
    <script src="SsoScripts/jquery.cookie.js" type="text/javascript"></script>
    <%--<script type="text/javascript">
        /**
        * @author mac_hou;
        * @date 2011,3,20;
        * @Copyright (c) 2011 AUTHORS.txt;
        * @Email blacksheep.hou@gmail.com; 
        **/
        //Begin jQuery
        $(function () {
            //append img to LI
            $("#nav-shadow li").append('<img class="shadow" src="images/reflaction_pic.jpg" width="60" height="32"  alt="" />');
            //append hover event
            $("#nav-shadow li").hover(function () {
                //define e for tihs
                var $e = this;
                $($e).find("a").stop().animate({ marginTop: '-14px' }, 250, function () {
                    $($e).find("a").animate({ marginTop: '-10px' }, 250);

                });
                $($e).find("img.shadow").stop().animate({ width: "80%", opacity: "0.3", marginLeft: "8px" }, 250);

            }, function () {
                var $e = this;
                $($e).find("a").stop().animate({ marginTop: "4px" }, 250, function () {
                    $($e).find("a").animate({ marginTop: "0px" }, 250);
                });
                $($e).find("img.shadow").stop().animate({ width: "100%", opacity: "1", marginLeft: "0px" }, 250);
            });
        })
        //end jQuery
    </script>
    <style type="text/css">
        ul, ol
        {
            list-style: none;
            list-style-type: none;
        }
        a, a:visited, a:hover
        {
            display: block;
            text-decoration: none;
            color: #ccc;
            text-indent: -9999px;
            outline: 0 none;
            width: 61px;
            height: 60px;
            z-index: 2;
            overflow: hidden;
            position: relative;
        }
        li
        {
            float: left;
            width: 61px;
            height: 92px;
            margin-left: 10px;
            position: relative;
        }
        #nav-shadow li.chang-one a
        {
            background: url(button_pic.jpg) no-repeat left top;
        }
        #nav-shadow li.chang-two a
        {
            background: url(button_pic.jpg) no-repeat -60px top;
        }
        #nav-shadow li.chang-three a
        {
            background: url(button_pic.jpg) no-repeat -120px top;
        }
        #nav-shadow li.chang-four a
        {
            background: url(button_pic.jpg) no-repeat -180px top;
        }
        #nav-shadow li.chang-five a
        {
            background: url(button_pic.jpg) no-repeat -240px top;
        }
        #nav-shadow li img.shadow
        {
            margin: 0 auto;
            position: absolute;
            bottom: 0px;
            left: 0px;
            z-index: 1;
        }
    </style>--%>
    <script type="text/javascript">

        $(document).ready(function () {
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "WhereToGoV2.aspx/GetSites",
                data: null,
                dataType: "json",
                success: function (result) {
                    var obj = result.d;
                    $.each(obj, function (i, item) {
                        $("#abcd").append(item);
                    });
                    $("#abcd").show();
                },
                error: function () { alert("程序异常"); }
            });

            var currentValue = $("#abcd").val();
            $("#abcd").change(function () {
                if (currentValue != $("#abcd").val()) {
                    currentValue = $("#abcd").val();
                    $.ajax({
                        type: "POST",
                        contentType: "application/json",
                        url: "WhereToGoV2.aspx/SetDefaultSite",
                        data: "{myDefault:'" + currentValue + "'}",
                        dataType: "json",
                        success: function (result) {
                            if (result.d == 'ok') {
                                alert("设定成功，你的默认站点修改为：" + currentValue);
                            } else {
                                alert("设定默认系统失败！");
                            }
                        },
                        error: function () { alert("程序异常,"); }
                    });
                }
            });



        });


        //        $(document).ready(function () {
        //            var provinceValue = $("#province").val();
        //            var data = [];
        //            data["province"] =
        //            {
        //                value: $("#province").val(),
        //                url: function () { return "/home/transfer/?province=" + window.encodeURI($("#province").val()) + "&t=" + Math.round(Math.random() * 10000) + "&callback=?"; }
        //            };

        //            data["city"] =
        //            {
        //                value: $("#city").val(),
        //                url: function () { return "/home/transfer/?province=" + window.encodeURI($("#province").val()) + "&city=" + window.encodeURI($("#city").val()) + "&t=" + Math.round(Math.random() * 10000) + "&callback=?"; }
        //            };

        //            $("#province,#city").change(function () {
        //                var id = this.id;
        //                if (data[id].value != $("#" + this.id).val()) {
        //                    data[id].value = $("#" + this.id).val();
        //                    if (data[id].value != "请选择所在省/直辖市" || data[id].value != "请选择所在城市") {
        //                        $.ajax({
        //                            type: "GET",
        //                            url: data[id].url(),
        //                            cache: false,
        //                            error: function (XMLHttpRequest, textStatus, errorThrown) {
        //                                alert(textStatus);
        //                                alert("程序出错,请联系管理员.");
        //                            },
        //                            dataType: "jsonp",
        //                            jsonp: 'callback',
        //                            success: function (json) {
        //                                var $this = $("#" + id).parent();
        //                                $this.nextAll().hide().find("option:gt(0)").remove();

        //                                $.each(json, function (i, item) {
        //                                    $this.next().children().append("<option>" + item + "</option>");
        //                                });
        //                                $this.next().show();
        //                            }

        //                        });


        //                    }
        //                }
        //            });
        //            var districtValue = $("#district").val()
        //            $("#district").change(function () {

        //                if (districtValue != $("#district").val()) {
        //                    districtValue = $("#district").val();
        //                    var url = [];
        //                    url.push("/home/transfer");
        //                    url.push("?province=");
        //                    url.push(window.encodeURI($("#province").val()));
        //                    url.push("&city=");
        //                    url.push(window.encodeURI($("#city").val()));
        //                    url.push("&district=")
        //                    url.push(window.encodeURI($(this).val()));
        //                    url.push("&t=");
        //                    url.push(Math.round(Math.random() * 10000));
        //                    url.push("&callback=?")
        //                    $.ajax({
        //                        type: "GET",
        //                        url: url.join(''),
        //                        cache: false,
        //                        dataType: 'html',
        //                        success: function (json) {
        //                            $('#display').html(json);
        //                        }

        //                    });
        //                }
        //            });
        //        });
      
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <%--<div style="clear: right; float: left; text-align:center;">
        <div style="margin-left: auto; margin-right: auto; margin-top: 200px;">
            <%=_navi%>
        </div>
    </div>--%>
    <div style="float: left; text-align: center;">
        <div style="width: 700px; margin-left: auto; margin-right: auto;">
            <span style="font-size: 14pt;">设定我的默认</span>
        </div>
    </div>
    <span>
        <select id="abcd">
        </select>
        <select id="province">
            <option>请选择所在省/直辖市</option>
            <option value="a" selected="selected">北京</option>
            <option>天津</option>
            <option>河北</option>
            <option>山西</option>
            <option>内蒙古</option>
            <option>辽宁</option>
            <option>吉林</option>
            <option>黑龙江</option>
            <option>上海</option>
            <option>江苏</option>
            <option>浙江</option>
            <option>安徽</option>
            <option>福建</option>
            <option>江西</option>
            <option>山东</option>
            <option>河南</option>
            <option>湖北</option>
            <option>湖南</option>
            <option>广东</option>
            <option>广西</option>
            <option>海南</option>
            <option>重庆</option>
            <option>四川</option>
            <option>贵州</option>
            <option>云南</option>
            <option>西藏</option>
            <option>陕西</option>
            <option>甘肃</option>
            <option>青海</option>
            <option>宁夏</option>
            <option>新疆</option>
        </select></span> <span>
            <select id="city" style="width: auto;">
                <option>请选择所在城市</option>
            </select></span> <span>
                <select id="district" style="width: auto;">
                    <option>请选择所在县/区</option>
                </select>
            </span>
    </form>
</body>
</html>
