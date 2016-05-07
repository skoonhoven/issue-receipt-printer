//Use http://mrcoles.com/bookmarklet/ to generate valid bookmarklet code
var issuePrinterHost = "{issueprinter.webservice.nl}";

var t = document.title.replace(" - JIRA", "");
var match = t.match(/\[([a-zA-Z]*-[0-9]*)\]/);
var key = "";

if (match) {
    key = match[1];
}

if (key !== "")
{
    document.body.innerHTML += '<img height="0" width="0" src="'+ issuePrinterHost +'/print/issue/'+key + '" />';
}
