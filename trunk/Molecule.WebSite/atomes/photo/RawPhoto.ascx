<%@ Control Language="C#" Inherits="Molecule.WebSite.atomes.photo.RawPhoto" %>
<%@ Import Namespace="Molecule.WebSite.atomes.photo" %>
<%@ Import Namespace="WebPhoto.Services" %>
<a href="<%= PhotoFile.GetUrlFor(PhotoId, PhotoFileSize.Raw) %>">
    <img src="images/zoom-original.png" /><asp:Literal runat="server" Text="<%$ Resources:OriginaleImage%>" />
</a>