﻿var MainFrame = document.getElementById("MainFrame");
var systemList = document.getElementById("SystemList");
var systemSwitchButton = document.getElementById("SystemSwitchButton");
window.onload = window.onresize = function() {
    resizeBody();
};

function resizeBody() {
    var bodyHeight = document.body.clientHeight;
    MainFrame.style.height = (bodyHeight - 60) + "px";
}
systemSwitchButton.onmouseover = function() {
    var top = this.offsetTop + this.offsetHeight;
    var left = this.offsetLeft;
    var obj = this;
    while (obj = obj.offsetParent) {
        top += obj.offsetTop;
        left += obj.offsetLeft;
    }
    systemList.style.left = left + "px";
    systemList.style.top = top + "px";
    systemList.style.display = "block";
};
systemSwitchButton.onmouseout = function() {
    systemList.style.display = "none";
};
