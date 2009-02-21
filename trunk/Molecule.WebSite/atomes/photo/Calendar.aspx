<%@ Page Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Calendar.aspx.cs" Inherits="Molecule.WebSite.atomes.photo.Calendar1" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
<asp:HyperLink ID="HyperLinkPrevious" runat="server">&lt;</asp:HyperLink>
<asp:Label ID="LabelMonth" runat="server" Text="" />
<asp:HyperLink ID="HyperLinkNext" runat="server">&gt;</asp:HyperLink>
<asp:ListView ID="ListView1" runat="server" EnableViewState="false"
    GroupItemCount="7">
    <EmptyItemTemplate>
        <td id="Td1" runat="server" />
        </EmptyItemTemplate>
        <ItemTemplate>
            <td id="Td2" runat="server">
                <div style="display:block">
                <asp:Image ID="img" runat="server" ImageUrl='<%# Eval("ThumbnailUrl") %>' />
                <div style="position:relative;"><asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Day") %>' /></div>
                
                </div>
            </td>
        </ItemTemplate>
        <EmptyDataTemplate>
            <table id="Table1" runat="server" style="">
                <tr>
                    <td>
                        No data was returned.</td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <table ID="groupPlaceholderContainer" runat="server">
            <thead>
                <tr>
                    <td id="Td3" runat="server"><%= FormatDay(0) %></td>
                    <td id="Td4" runat="server"><%= FormatDay(1) %></td>
                    <td id="Td5" runat="server"><%= FormatDay(2) %></td>
                    <td id="Td6" runat="server"><%= FormatDay(3) %></td>
                    <td id="Td7" runat="server"><%= FormatDay(4) %></td>
                    <td id="Td8" runat="server"><%= FormatDay(5) %></td>
                    <td id="Td9" runat="server"><%= FormatDay(6) %></td>
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
