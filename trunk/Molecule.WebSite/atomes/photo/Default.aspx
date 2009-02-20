<%@ Page Title="" Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Molecule.WebSite.atomes.photo.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <asp:DataList ID="DataList1" runat="server" DataSourceID="PhotoDataSource">
        <ItemTemplate>
            Tags:
            <asp:Label ID="TagsLabel" runat="server" Text='<%# Eval("Tags") %>' />
            <br />
            Id:
            <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
            <br />
            MediaFilePath:
            <asp:Label ID="MediaFilePathLabel" runat="server" 
                Text='<%# Eval("MediaFilePath") %>' />
            <br />
            Metadatas:
            <asp:Label ID="MetadatasLabel" runat="server" Text='<%# Eval("Metadatas") %>' />
            <br />
            Date:
            <asp:Label ID="DateLabel" runat="server" Text='<%# Eval("Date") %>' />
            <br />
            <br />
        </ItemTemplate>
    </asp:DataList>
    <asp:ObjectDataSource ID="PhotoDataSource" runat="server" 
        SelectMethod="GetPhotos" TypeName="WebPhoto.Services.PhotoLibrary">
    </asp:ObjectDataSource>
</asp:Content>
