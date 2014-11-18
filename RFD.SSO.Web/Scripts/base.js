function include() {
	for (var i = 0; i < arguments.length; i++) {
		var file = arguments[i];
		if (file.match(/\.js$/i))
			document.write('<script type=\"text/javascript\" src=\"' + file + '\"></sc' + 'ript>');
		else
			document.write('<style type=\"text/css\">@import \"' + file + '\" ;</style>');
	}
};