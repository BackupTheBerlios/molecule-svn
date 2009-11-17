<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Molecule.MvcWebSite.atomes.music.Data.IndexData>" %>
<%@ Import Namespace="Molecule.MvcWebSite.atomes.music.Controllers" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

<head runat="server">
    <title></title>
    <link rel="shortcut icon" href="~/favicon.ico" type="image/x-icon" />
    <link rel="icon" href="~/favicon.ico" type="image/ico" />
    <link type="text/css" rel="stylesheet" href="<%= Url.Theme("molecule.css") %>" />
</head>
<body>

    <div>
        <center><%= ViewData["ErrorMessage"] %></center>
        
        <%  if (((bool) ViewData["ShowDownloadLink"] ) == true)
            {%>
               <center> <a  href="<%= "../../Archive/"+ViewData["DownloadId"]  %>">Click Here to download the archive</a></center>
        <% } %>
    </div>
</body>
</html>
