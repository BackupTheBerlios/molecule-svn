<%@ Page Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Timeline.aspx.cs"
 Inherits="Molecule.WebSite.atomes.photo.Timeline" EnableViewState="false" %>
 <%@ Import Namespace="Molecule.WebSite.atomes.photo" %>
<%@ Register Src="TagList.ascx" TagName="TagList" TagPrefix="photo" %>
<%@ Register src="PhotoLink.ascx" tagname="PhotoLink" tagprefix="photo" %>
<%@ Import Namespace="WebPhoto.Services" %>
<%@ Import Namespace="WebPhoto.Providers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="style/common.css" rel="stylesheet" type="text/css" />
    <link href="style/timeline.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <asp:ListView runat="server" ID="ListView1">
        <ItemTemplate>
            <li class="PhotoItem">
                <photo:PhotoLink runat="server" PhotoId='<%# Eval("Id") %>' />
                <asp:HyperLink runat="server" NavigateUrl='<%# MonthCalendar.GetUrlFor((DateTime)Eval("Date"), null) %>'>
                <%# ((DateTime)Eval("Date")).ToShortDateString() %>
                </asp:HyperLink>
                 - <photo:TagList runat="server" Tags='<%# PhotoLibrary.GetTagsByPhoto((string)Eval("Id")) %>' />
                <p><%# Eval("Description") %></p>
            </li>
        </ItemTemplate>
        <LayoutTemplate>
        <ul>
            <li runat="server" id="itemPlaceHolder">
            </li>
        </ul>
        </LayoutTemplate>
    </asp:ListView>
</asp:Content>
