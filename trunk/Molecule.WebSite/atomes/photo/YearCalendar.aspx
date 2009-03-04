﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="YearCalendar.aspx.cs" Inherits="Molecule.WebSite.atomes.photo.YearCalendar" %>
<%@ Import Namespace="Molecule.WebSite.atomes.photo" %>
<%@ Register src="PhotoLink.ascx" tagname="PhotoLink" tagprefix="photo" %>
<%@ Register src="TagHierarchy.ascx" tagname="TagHierarchy" tagprefix="photo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="style/calendar.css" rel="stylesheet" type="text/css" />
    <link href="style/common.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <photo:TagHierarchy ID="TagHierarchy" runat="server" TagQueryStringField="tag"/>
    <asp:ListView ID="ListView1" runat="server" GroupItemCount="6">
    <ItemTemplate>
        <td>
            <photo:PhotoLink runat="server" Description='<%# Eval("Description") %>'
                 HoverText='<%# Eval("Name") %>' TagId='<%# tagId %>' PhotoId='<%# Eval("PhotoId") %>'
                 NavigateUrl='<%# MonthCalendar.GetUrlFor((DateTime)Eval("DateTime"), tagId) %>' />
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
