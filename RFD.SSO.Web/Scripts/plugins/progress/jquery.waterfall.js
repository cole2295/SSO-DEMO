/*FileName: jquery.waterfall.js
**Author:   何名宇
**Date:     2011-09-15
**Usage:    带进度条的遮罩层插件
**/
(function($) {
    $.fn.progressDialog = function() {

    };

    $.fn.progressDialog.showDialog = function(text) {
        text = text || "加载中，请稍后..."
        createElement(text);
        setPosition();
        waterfall.appendTo("body");
        $(window).bind('resize', function() {
            setPosition();
        });
    }

    $.fn.progressDialog.hideDialog = function(text) {
        if (!waterfall)
            waterall = $("#waterfall");
        if (waterfall) {
            waterfall.remove();
        }
        

    }

    function createElement(text) {
        if (!waterfall) {
            waterfall = $(document.createElement("div"));
            waterfall.attr("id", "waterfall");
            waterfall.css({
                "height": "100%",
                "width": "100%",
                "filter": "alpha(opacity = 50)",
                "-moz-opacity": "0.5",
                "opacity": "0.5",
                "background-color": "#CCCCCC",
                "position": "absolute",
                "left": "0px",
                "top": "0px"
            });
        }
       
        if (!loadDiv) {
            loadDiv = document.createElement("div");
        }
        $(loadDiv).appendTo(waterfall);

        var content = " <div style='width:" + width + "px; height:" + Height + "px;'><div style='width:100%; height:40px; line-height:41px;padding-left:10px;font-weight:bolder; color:#006600;'>" + text + "</div><div align='center'><img src='../Scripts/imgs/loading.gif' border='0'/></div></div>";
        $(loadDiv).html(content);
    }

    function setPosition() {
        var leftOffset = ($(document).width() - width) / 2;
        var topOffset = ($(document).height() - Height) / 2;
        $(loadDiv).css({
            "position": "absolute",
            "height": Height + "px",
            "width": width + "px",
            "left": leftOffset + "px",
            "top": topOffset + "px"
        });
    }

    var waterfall;
    var loadDiv;
    var width = 290;
    var Height = 60;
})(jQuery);