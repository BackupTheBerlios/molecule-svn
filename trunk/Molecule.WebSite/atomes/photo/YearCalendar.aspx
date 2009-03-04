<%@ Page Title="" Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="YearCalendar.aspx.cs" Inherits="Molecule.WebSite.atomes.photo.YearCalendar" %>
<%@ Register src="PhotoLink.ascx" tagname="PhotoLink" tagprefix="photo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="style/calendar.css" rel="stylesheet" type="text/css" />
    <link href="style/common.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <asp:ListView ID="ListView1" runat="server">
    <ItemTemplate>
        <td>
            <photo:PhotoLink runat="server" Description='<%# Eval("PhotoCount") + " photos" %>'
                 HoverText='<%# Eval("Month") %>' TagId='<%# tagId %>' PhotoId='<%# Eval("PhotoId") %>' />
        </td>
    </ItemTemplate>
    <LayoutTemplate>
        <table>
            <tbody>
                <tr ID="groupPlaceholder" runat="server"></tr>
            </tbody>
        </table>
    </LayoutTemplate>
    <GroupTemplate>
        <tr ID="itemPlaceholderContainer" runat="server">
            <td ID="itemPlaceholder" runat="server">
            </td>
        </tr>
    </GroupTemplate>
    </asp:ListView>
</asp:Content>
