<%@ Page Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Photo.aspx.cs"
    EnableEventValidation="false" Inherits="Molecule.WebSite.atomes.photo.Photo" %>
<%@ Import Namespace="Molecule.WebSite.atomes.photo" %>
<%@ Import Namespace="WebPhoto.Services" %>
<%@ Import Namespace="WebPhoto.Providers" %>
<%@ Register Src="TagList.ascx" TagName="TagList" TagPrefix="photo" %>
<%@ Register src="TagHierarchy.ascx" tagname="TagHierarchy" tagprefix="photo" %>
<%@ Register src="PhotoLink.ascx" tagname="PhotoLink" tagprefix="photo" %>
<%@ Register Src="FullSizePhoto.ascx" TagName="Photo" TagPrefix="photo" %>
<%@ Register Src="Map.ascx" TagName="Map" TagPrefix="photo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="style/common.css" rel="stylesheet" type="text/css" />
    <link href="style/photo.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy" runat="server">
        <Scripts>
            <asp:ScriptReference Path="scripts/default.js" />
        </Scripts>
    </asp:ScriptManagerProxy>
    <nStuff:UpdateHistory runat="server" ID="updateHistory"
        OnNavigate="OnUpdateHistoryNavigate" />
    
    <photo:TagHierarchy ID="TagHierarchy" runat="server" TagQueryStringField="tag" />
    
    <nStuff:StyledUpdatePanel runat="server" ID="mainUP" CssClass="photoCurrent" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ImageButton ID="PreviousPhotoLink" runat="server" ImageUrl="~/App_Themes/bloup/images/go-previous.png"
             OnClick="OnPreviousClick"/>
            <asp:ImageButton ID="NextPhotoLink" runat="server" ImageUrl="~/App_Themes/bloup/images/go-next.png"
             OnClick="OnNextClick"/>
            <br />
            <photo:Photo ID="FullSizePhoto" runat="server" /> 
            <div id="photoDescriptionContainer">
                <p>
                    <asp:Label ID="LabelDescription" runat="server"/>
                </p>
                <photo:TagList runat="server" ID="tagList" />            
                <photo:Map ID="PhotoMap" runat="server" />
            </div>
        </ContentTemplate>
    </nStuff:StyledUpdatePanel>
</asp:Content>
