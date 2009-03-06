<%@ Page Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="MonthCalendar.aspx.cs"
Inherits="Molecule.WebSite.atomes.photo.MonthCalendar" Title="Untitled Page" EnableViewState="false" %>
<%@ Import Namespace="Molecule.WebSite.atomes.photo" %>
<%@ Register src="TagHierarchy.ascx" tagname="TagHierarchy" tagprefix="photo" %>
<%@ Register src="PhotoLink.ascx" tagname="PhotoLink" tagprefix="photo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="style/calendar.css" rel="stylesheet" type="text/css" />
    <link href="style/common.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <photo:TagHierarchy ID="TagHierarchy" runat="server" TagQueryStringField="tag"/>
    <br />
    <div class="BlockItem">
        <asp:HyperLink ID="HyperLinkPrevious" runat="server">
            <img style="border:none" src="../../App_Themes/<%=Theme %>/images/go-previous.png" />
        </asp:HyperLink>
    </div>
    <div class="BlockItem">
        <asp:ListView ID="ListView1" runat="server" EnableViewState="false" GroupItemCount="7">
            <ItemTemplate>
                <td>
                    <photo:PhotoLink runat="server" Description='<%# Eval("Description") %>'
                     HoverText='<%# Eval("Name") %>' TagId='<%# tagId %>' PhotoId='<%# Eval("PhotoId") %>' />
                </td>
            </ItemTemplate>
            <LayoutTemplate>
                <table style="border-collapse:collapse">
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
    </div>
    <div class="BlockItem">
        <asp:HyperLink ID="HyperLinkNext" runat="server">
            <img style="border:none" src="../../App_Themes/<%=Theme %>/images/go-next.png" />
        </asp:HyperLink>
    </div>
</asp:Content>
