<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FullSizePhoto.ascx.cs"
    Inherits="Molecule.WebSite.atomes.photo.FullSizePhoto" %>
<%@ Import Namespace="WebPhoto.Services" %>
<%@ Import Namespace="Molecule.WebSite.atomes.photo" %>

<script type="text/javascript">
    function showElement(id) {
        var meta = document.getElementById(id);
        meta.style.display = 'block';
    }
    function hideElement(id) {
        var meta = document.getElementById(id);
        meta.style.display = 'none';
    }
</script>

<div id="photo">
    <div id="metadatas" onmouseover="showElement('metadatas');">
        <asp:GridView ID="MetadatasGridView" runat="server" AutoGenerateColumns="true" ShowHeader="false">
        </asp:GridView>
    </div>
    <img alt="<%= PhotoId %>" src="<%=PhotoFile.GetUrlFor(PhotoId, PhotoFileSize.Normal) %>"
        onmouseover="showElement('metadatas');" onmouseout="hideElement('metadatas');" />
</div>
