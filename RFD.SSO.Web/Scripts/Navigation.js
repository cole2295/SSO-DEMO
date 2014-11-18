﻿function $(id) { return document.getElementById(id); };
$('Switch').onmouseover = function() {
    this.style.backgroundColor = '#FFDDAA';
    this.style.borderColor = '#990000';
};
$('Switch').onmouseout = function() {
    this.style.backgroundColor = '#F5F5F5';
    this.style.borderColor = '#FFCC99';
};
$('Switch').onclick = ShowHideNavigation;
//显示隐藏导航
var IsNavShow = true;
function ShowHideNavigation() {
    var nav = window.parent.document.getElementById("Navigation");
    if (IsNavShow) {
        nav.cols = "0,8,*";
        $('Swi').src = '/Scripts/Images/switch_2.gif';
        IsNavShow = false;
    }
    else {
        nav.cols = "200,8,*";
        $('Swi').src = '/Scripts/Images/switch_1.gif';
        IsNavShow = true;
    }
}
