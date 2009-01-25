function init()
{

	var uploadProgressBar = NeatUploadPB.prototype.Bars["uploadProgressBar"];
	var origDisplay = inlineProgressBar.Display;
	uploadProgressBar.Display = function()
	{
		var elem = document.getElementById(this.ClientID);
		elem.parentNode.style.display = "block";
		origDisplay.call(this);
	}
	uploadProgressBar.EvalOnClose = "NeatUploadMainWindow.document.getElementById('" 
		+ uploadProgressBar.ClientID + "').parentNode.style.display = \"none\";";
}