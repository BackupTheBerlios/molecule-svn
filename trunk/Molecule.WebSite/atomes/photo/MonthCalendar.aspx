<%@ Page Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="MonthCalendar.aspx.cs"
Inherits="Molecule.WebSite.atomes.photo.MonthCalendar" Title="Untitled Page" EnableViewState="false" %>
<%@ Register src="TagHierarchy.ascx" tagname="TagHierarchy" tagprefix="photo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="style/calendar.css" rel="stylesheet" type="text/css" />
    <link href="style/common.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <photo:TagHierarchy ID="TagHierarchy" runat="server" TagQueryStringField="tag" /> > <asp:Label ID="LabelMonth" runat="server" Text="" />
    <div style="display:block; position:relative">
        <div style="float:left;"><asp:HyperLink ID="HyperLinkPrevious" runat="server">PreviousMonth</asp:HyperLink></div>
        <div style="float:right;"><asp:HyperLink ID="HyperLinkNext" runat="server">NextMonth</asp:HyperLink></div>
    </div>
    <br />
    <asp:ListView ID="ListView1" runat="server" EnableViewState="false" GroupItemCount="7">
        <EmptyItemTemplate>
            <td runat="server" />
        </EmptyItemTemplate>
        <ItemTemplate>
            <td runat="server" class="calendarItem">
                <asp:PlaceHolder runat="server" Visible='<%# Eval("IsCurrentMonth") %>'>
                    <div class="number"><asp:Literal runat="server" Visible='<%# Eval("HasThumbnail")%>' Text='<%# Eval("Day") %>' /></div>
                    <div class="numberShadow"><asp:Literal runat="server" Text='<%# Eval("Day") %>' /></div>
                    <asp:HyperLink runat="server" Visible='<%# Eval("HasThumbnail")%>' NavigateUrl='<%# Eval("NavigateUrl") %>'>
                        <img title='<%# Eval("PhotoCount") + " photos" %>' class="photoLink" runat="server" src='<%# Eval("ThumbnailUrl") %>' />
                    </asp:HyperLink>
                </asp:PlaceHolder>
            </td>
        </ItemTemplate>
        <EmptyDataTemplate>
            <td></td>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <table style="">
            <thead>
                <tr>
                    <td runat="server"><%= FormatDay(0) %></td>
                    <td runat="server"><%= FormatDay(1) %></td>
                    <td runat="server"><%= FormatDay(2) %></td>
                    <td runat="server"><%= FormatDay(3) %></td>
                    <td runat="server"><%= FormatDay(4) %></td>
                    <td runat="server"><%= FormatDay(5) %></td>
                    <td runat="server"><%= FormatDay(6) %></td>
                </tr>
            </thead>
                <tbody>
                    <tr ID="groupPlaceholder" runat="server">
                    </tr>
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
