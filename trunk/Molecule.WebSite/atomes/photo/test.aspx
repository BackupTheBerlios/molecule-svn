<%@ Page Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="Molecule.WebSite.atomes.photo.test" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <asp:ListView ID="ListView1" runat="server" DataSourceID="ObjectDataSource1">
        <ItemTemplate>
            <tr style="">
                <td>
                    <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
                </td>
                <td>
                    <asp:Label ID="MediaFilePathLabel" runat="server" 
                        Text='<%# Eval("MediaFilePath") %>' />
                </td>
                <td>
                    <asp:Label ID="MetadatasLabel" runat="server" Text='<%# Eval("Metadatas") %>' />
                </td>
                <td>
                    <asp:Label ID="DateLabel" runat="server" Text='<%# Eval("Date") %>' />
                </td>
                <td>
                    <asp:Label ID="DescriptionLabel" runat="server" 
                        Text='<%# Eval("Description") %>' />
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr style="">
                <td>
                    <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
                </td>
                <td>
                    <asp:Label ID="MediaFilePathLabel" runat="server" 
                        Text='<%# Eval("MediaFilePath") %>' />
                </td>
                <td>
                    <asp:Label ID="MetadatasLabel" runat="server" Text='<%# Eval("Metadatas") %>' />
                </td>
                <td>
                    <asp:Label ID="DateLabel" runat="server" Text='<%# Eval("Date") %>' />
                </td>
                <td>
                    <asp:Label ID="DescriptionLabel" runat="server" 
                        Text='<%# Eval("Description") %>' />
                </td>
            </tr>
        </AlternatingItemTemplate>
        <EmptyDataTemplate>
            <table runat="server" style="">
                <tr>
                    <td>
                        No data was returned.</td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <InsertItemTemplate>
            <tr style="">
                <td>
                    <asp:Button ID="InsertButton" runat="server" CommandName="Insert" 
                        Text="Insert" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" 
                        Text="Clear" />
                </td>
                <td>
                    <asp:TextBox ID="IdTextBox" runat="server" Text='<%# Bind("Id") %>' />
                </td>
                <td>
                    <asp:TextBox ID="MediaFilePathTextBox" runat="server" 
                        Text='<%# Bind("MediaFilePath") %>' />
                </td>
                <td>
                    <asp:TextBox ID="MetadatasTextBox" runat="server" 
                        Text='<%# Bind("Metadatas") %>' />
                </td>
                <td>
                    <asp:TextBox ID="DateTextBox" runat="server" Text='<%# Bind("Date") %>' />
                </td>
                <td>
                    <asp:TextBox ID="DescriptionTextBox" runat="server" 
                        Text='<%# Bind("Description") %>' />
                </td>
            </tr>
        </InsertItemTemplate>
        <LayoutTemplate>
            <table runat="server">
                <tr runat="server">
                    <td runat="server">
                        <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                            <tr runat="server" style="">
                                <th runat="server">
                                    Id</th>
                                <th runat="server">
                                    MediaFilePath</th>
                                <th runat="server">
                                    Metadatas</th>
                                <th runat="server">
                                    Date</th>
                                <th runat="server">
                                    Description</th>
                            </tr>
                            <tr ID="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server">
                    <td runat="server" style="">
                        <asp:DataPager ID="DataPager1" runat="server">
                            <Fields>
                                <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" 
                                    ShowLastPageButton="True" />
                            </Fields>
                        </asp:DataPager>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
        <EditItemTemplate>
            <tr style="">
                <td>
                    <asp:Button ID="UpdateButton" runat="server" CommandName="Update" 
                        Text="Update" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" 
                        Text="Cancel" />
                </td>
                <td>
                    <asp:TextBox ID="IdTextBox" runat="server" Text='<%# Bind("Id") %>' />
                </td>
                <td>
                    <asp:TextBox ID="MediaFilePathTextBox" runat="server" 
                        Text='<%# Bind("MediaFilePath") %>' />
                </td>
                <td>
                    <asp:TextBox ID="MetadatasTextBox" runat="server" 
                        Text='<%# Bind("Metadatas") %>' />
                </td>
                <td>
                    <asp:TextBox ID="DateTextBox" runat="server" Text='<%# Bind("Date") %>' />
                </td>
                <td>
                    <asp:TextBox ID="DescriptionTextBox" runat="server" 
                        Text='<%# Bind("Description") %>' />
                </td>
            </tr>
        </EditItemTemplate>
        <SelectedItemTemplate>
            <tr style="">
                <td>
                    <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
                </td>
                <td>
                    <asp:Label ID="MediaFilePathLabel" runat="server" 
                        Text='<%# Eval("MediaFilePath") %>' />
                </td>
                <td>
                    <asp:Label ID="MetadatasLabel" runat="server" Text='<%# Eval("Metadatas") %>' />
                </td>
                <td>
                    <asp:Label ID="DateLabel" runat="server" Text='<%# Eval("Date") %>' />
                </td>
                <td>
                    <asp:Label ID="DescriptionLabel" runat="server" 
                        Text='<%# Eval("Description") %>' />
                </td>
            </tr>
        </SelectedItemTemplate>
    </asp:ListView>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True" 
        SelectMethod="GetPhotos" TypeName="WebPhoto.Services.PhotoLibrary">
    </asp:ObjectDataSource>
    <p>
    </p>
</asp:Content>
