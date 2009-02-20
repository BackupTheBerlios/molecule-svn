<%@ Page Title="" Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Molecule.WebSite.atomes.photo.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <asp:ListView ID="ListView1" runat="server" DataSourceID="PhotoDataSource">
            <ItemTemplate>
                <span style=""><asp:Image ID="Image1" runat="server" ImageUrl='<%#  "Thumbnail.aspx?id="+Eval("Id") %>' />
                </span>
            </ItemTemplate>
            
            <EmptyDataTemplate>
                <span>No data was returned.</span>
            </EmptyDataTemplate>
            
            <LayoutTemplate>
                <div ID="itemPlaceholderContainer" runat="server" style="">
                    <span ID="itemPlaceholder" runat="server" />
                </div>
                <div style="">
                    <asp:DataPager ID="DataPager1" runat="server">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" 
                                ShowNextPageButton="False" ShowPreviousPageButton="False" />
                            <asp:NumericPagerField />
                            <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="True" 
                                ShowNextPageButton="False" ShowPreviousPageButton="False" />
                        </Fields>
                    </asp:DataPager>
                </div>
            </LayoutTemplate>
            <EditItemTemplate>
                <span style="">Tags:
                <asp:TextBox ID="TagsTextBox" runat="server" Text='<%# Bind("Tags") %>' />
                <br />
                Id:
                <asp:TextBox ID="IdTextBox" runat="server" Text='<%# Bind("Id") %>' />
                <br />
                MediaFilePath:
                <asp:TextBox ID="MediaFilePathTextBox" runat="server" 
                    Text='<%# Bind("MediaFilePath") %>' />
                <br />
                Metadatas:
                <asp:TextBox ID="MetadatasTextBox" runat="server" 
                    Text='<%# Bind("Metadatas") %>' />
                <br />
                Date:
                <asp:TextBox ID="DateTextBox" runat="server" Text='<%# Bind("Date") %>' />
                <br />
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" 
                    Text="Update" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" 
                    Text="Cancel" />
                <br />
                <br />
                </span>
            </EditItemTemplate>
            <SelectedItemTemplate>
                <span style="">Tags:
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
                </span>
            </SelectedItemTemplate>
        </asp:ListView>
    <asp:ObjectDataSource ID="PhotoDataSource" runat="server" 
        SelectMethod="GetPhotos" TypeName="WebPhoto.Services.PhotoLibrary">
    </asp:ObjectDataSource>
</asp:Content>
