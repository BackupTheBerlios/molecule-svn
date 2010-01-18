<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Page.Master"
    Inherits="System.Web.Mvc.ViewPage<Molecule.Atomes.Documents.Data.FolderDisplayData>" %>
    <%@ Import Namespace="Molecule.Atomes.Documents.Controllers" %>
    <%@ Import Namespace="Molecule.Atomes.Documents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<title>Template atome</title>
<script type="text/javascript" src="../../../../Scripts/jquery-1.3.2.js"></script>
<script type="text/javascript" src="../../../../Scripts/jquery.tools.min.js"></script>
<script type="text/javascript">
    $(document).ready(function() {
        
    });
</script>
<style type="text/css">
ul
{
    list-style-type: none;padding: 0px;margin: 0px;
}

li
{
	background-repeat: no-repeat;padding-left: 24px; margin:3px;
}

#folderList li { background-image: url(<%=Url.Theme("images/folder.png") %>); }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
<h2><%= Html.ActionLink<FolderController>("Documents", c => c.Display(null))%>
    <%foreach (var folder in Model.CurrentFolderHierarchy) { %>
       > 
      <%= Html.ActionLink<FolderController>(folder.Name, c => c.Display(folder.Id)) %>
    <% } %>
</h2>
<%= Html.ActionLink<FolderController>("Créer un dossier", c => c.Create(Model.CurrentFolder.Id))%><br />
<%= Html.ActionLink<FolderController>("Ajouter un document", c => c.AddDocument(Model.CurrentFolder.Id))%><br />
<br />
<% Func<string, string> mimeIconProvider = (fileName) => {
           switch (System.IO.Path.GetExtension(fileName.ToLower()).Substring(1)){
               case "png":
               case "jpg":
               case "gif":
               case "bmp":
               case "tiff":
               case "jpeg":
               case "ps":
                   return "image-x-generic";
               case "cer":
                   return "application-certificate";
               case "exe":
               case "jar":
               case "msi":
               case "dll":
               case "bin":
                   return "application-x-executable";
               case "ogg":
               case "mp3":
               case "mpa":
               case "wma":
                   return "audio-x-generic";
               case "ttf":
                   return "font-x-generic";
               case "zip":
               case "gz":
               case "tar":
               case "deb":
               case "rpm":
               case "bz2":
               case "7z":
               case "rar":
               case "cab":
               case "iso":
                   return "package-x-generic";
               case "html":
               case "htm":
                   return "text-html";
               case "txt":
                   return "text-x-generic";
               case "js":
               case "vbs":
               case "py":
               case "rb":
               case "sh":
                   return "text-x-script";
               case "avi":
               case "mp4":
               case "ogv":
               case "divx":
               case "wmv":
               case "mpg":
               case "mpeg":
                   return "video-x-generic";
               case "wab":
                   return "x-office-address-book";
               case "cal":
                   return "x-office-calendar";
               case "dot":
               case "dotx":
               case "ott":
                   return "x-office-document-template";
               case "doc":
               case "odt":
               case "docx":
                   return "x-office-document";
               case "otg":
               return "x-office-drawing-template";
               case "odg":
               case "svg":
               case "wmf":
               case "emf":
                   return "x-office-drawing";
               case "pot":
               case "potx":
               case "otp":
                   return "x-office-presentation-template";
               case "ppt":
               case "pps":
               case "pptx":
               case "odp":
                   return "x-office-presentation";
               case "xlt":
               case "xltx":
               case "ots":
                   return "x-office-spreadsheet-template";
               case "xls":
               case "csv":
               case "xlsx":
               case "ods":
                   return "x-office-spreadsheet";
               default:
                   return "text-x-generic-template";

           }
       }; %>


<ul id="folderList">
<% foreach (var folder in Model.Folders) { %>
    <li><%= Html.ActionLink<FolderController>(folder.Name, c => c.Display(folder.Id)) %></li>
<%} %>
</ul>
<ul id="documentList">
<% foreach (var doc in Model.Documents) { %>
    <li style="background-image: url(<%=Url.Theme("images/mimetypes/"+ mimeIconProvider(doc.Name)+".png") %>) ">
        <%= Html.ActionLink<FolderController>(doc.Name, c => c.File(doc.Id)) %></li>
<%} %>
</ul>
</asp:Content>
