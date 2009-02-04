<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebMusic._Default"
    MasterPageFile="~/Page.Master" EnableViewState="false" EnableTheming="true" EnableEventValidation="false" %>
    
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="WebMusic.Providers" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <link href="style/layout.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy" runat="server">
        <Scripts>
            <asp:ScriptReference Path="scripts/default.js" />
            <%--<asp:ScriptReference Path="scripts/soundmanager2.js" />--%>
            <asp:ScriptReference Path="scripts/soundmanager2-nodebug-jsmin.js" />
            <asp:ScriptReference Path="scripts/sm2player.js" />
        </Scripts>
    </asp:ScriptManagerProxy>
    <div id="fileNotFoundPanel" style="visibility:hidden">
        <img alt="Warning" src="images/dialog-warning.png" />
        <asp:Label runat="server" ID="LabelSongError" Text="<%$ Resources:SongError %>" />
    </div>
    <div id="playerpanel">
        <div id="playerControls">
            <img id="previousButton" alt="Précédent" src="images/media-skip-backward.png" onclick="return previousButton_onclick()" />&nbsp;
            <img id="playButton" alt="Jouer" src="images/media-playback-start.png" onclick="return playButton_onclick()" />
            <img id="pauseButton" alt="Pause" src="images/media-playback-pause.png" style="display: none;"
                onclick="return pauseButton_onclick()" />
            <img id="nextButton" alt="Suivant" src="images/media-skip-forward.png" onclick="return nextButton_onclick()" />
            <br />
            <div id="currentSongInfoLabel" style="font-size:120%">
            </div>
            <div id="currentSongPositionLabel" style="font-family:Courier New">
            </div>
        </div>
        <div id="playlistcontainer">
            <h2>
                <asp:Label runat="server" ID="playlistLabel" Text="<%$ Resources:Playlist %>" /></h2>
            <select size="10" multiple="multiple" id="playListBox" ondblclick="playSelectedSong()"
                onkeydown="playlist_onkeydown(event)"></select> 
            <input id="repeatAllCheckBox" type="checkbox"/><label for="repeatAllCheckBox"><asp:Literal ID="repeatAllCheckBoxLiteral" runat="server" Text="<%$ Resources:RepeatAll%>" /></label>
        </div>
    </div>
    
            <div id="navigationPanel">
                <div id="artistscontainer">
                    <h2>
                        <asp:Label runat="server" ID="aristsLabel2" Text="<%$ Resources:Artists %>" /></h2>
                    
                    <div id="artistList" style="overflow: auto; height:150px;" class="thinBox">
                        <asp:DataList ID="arl" runat="server" DataSourceID="ArtistDataSource" 
                            DataKeyField="Id" onselectedindexchanged="ArtistList_SelectedIndexChanged" CssClass="itemList">
                            <ItemTemplate>
                                <asp:LinkButton ID="aib" runat="server" Text='<%# Eval("Name") %>'
                                    CommandArgument='<%# Eval("Id") %>' 
                                    CommandName="Select" />
                            </ItemTemplate>
                            <SelectedItemTemplate>
                                <asp:Label ID="ail" runat="server" Text='<%# Eval("Name") %>' />
                            </SelectedItemTemplate>
                        </asp:DataList>
                        <asp:ObjectDataSource ID="ArtistDataSource" runat="server" 
                            SelectMethod="GetArtists" TypeName="WebMusic.Services.MusicLibrary">
                        </asp:ObjectDataSource>
                    </div>
                </div>
                <div id="albumscontainer">
                        <h2>
                            <asp:Label ID="labelAlbums" runat="server" Text="<%$ Resources:Albums %>" />
                        </h2>
                            
                      <div id="albumList" style="overflow: auto; height:150px;" class="thinBox">
                      <asp:UpdatePanel ID="albumUpdatePanel" UpdateMode="Conditional" runat="server" ChildrenAsTriggers="false">
                      <ContentTemplate>
                        <asp:DataList ID="all" runat="server" DataSourceID="AlbumDataSource"
                              DataKeyField="Id" onselectedindexchanged="AlbumList_SelectedIndexChanged" CssClass="itemList">
                            <ItemTemplate>
                                <asp:LinkButton ID="aib" runat="server" Text='<%# Eval("Name") %>'
                                    CommandArgument='<%# Eval("Id") %>' 
                                    CommandName="Select" />
                            </ItemTemplate>
                            <SelectedItemTemplate>
                             <asp:Label ID="ail" runat="server" Text='<%# Eval("Name") %>' />
                            </SelectedItemTemplate>
                        </asp:DataList>
                        <asp:ObjectDataSource ID="AlbumDataSource" runat="server" 
                            SelectMethod="GetAlbumsByArtist" TypeName="WebMusic.Services.MusicLibrary">
                            <SelectParameters>
                                <asp:SessionParameter Name="artist" SessionField="music.currentArtist" 
                                    Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        </ContentTemplate>
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="arl" EventName="ItemCommand" />
                        </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    </div>
            </div>
            <div id="songscontainer">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
                <table id="songsView">
                    <asp:Repeater ID="sv" runat="server" DataSourceID="SongDataSource">
                        <HeaderTemplate>
                            <thead>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="til" Text="<%$ Resources:Title %>" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="arl" Text="<%$ Resources:Artist %>" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="all" Text="<%$ Resources:Album %>" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="trl" Text="<%$ Resources:Track %>" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="dl" Text="<%$ Resources:Duration %>" />
                                    </td>
                                    <td style="text-align:right">
                                        <asp:Image runat="server" AlternateText="<%$ Resources:PlayAll %>" ToolTip="<%$ Resources:PlayAll %>" ImageUrl="images/media-playback-start-small.png" onclick="songsView_onclick('playAll')" />
                                        <asp:Image runat="server" AlternateText="<%$ Resources:EnqueueAll %>" ToolTip="<%$ Resources:EnqueueAll %>" ImageUrl="images/list-add.png" onclick="songsView_onclick('enqueueAll')" />
                                        <asp:Image runat="server" AlternateText="<%$ Resources:DownloadAll %>" ToolTip="<%$ Resources:DownloadAll %>" ImageUrl="images/document-save.png" onclick="songsView_onclick('downloadAll')"/>
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="display: none"><%# Server.UrlEncode(((ISong)(Container.DataItem)).Id)%></td>
                                <td>
                                    <%# Server.HtmlEncode(((ISong)Container.DataItem).Title) %>
                                </td>
                                <td>
                                    <%# Server.HtmlEncode(((ISong)Container.DataItem).Artist.Name) %>
                                </td>
                                <td>
                                    <%# Server.HtmlEncode(((ISong)Container.DataItem).Album.Name) %>
                                </td>
                                <td>
                                    <%# Eval("AlbumTrack") %>
                                </td>
                                <td>
                                    <%# FormatDuration(((ISong)Container.DataItem).Duration) %>
                                </td>
                                <td style="text-align:right">
                                    <img alt="" src="images/media-playback-start-small.png" onclick="songsViewItem_onclick(this,'play')" />
                                    <img alt="" src="images/list-add.png" onclick="songsViewItem_onclick(this,'enqueue')" />
                                    <a href="Download.aspx?songId=<%# Server.UrlEncode(((ISong)(Container.DataItem)).Id)%>"><img alt="" src="images/document-save.png"/></a>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody>
                        </FooterTemplate>
                    </asp:Repeater>
                    <asp:ObjectDataSource ID="SongDataSource" runat="server" 
                        SelectMethod="GetSongsByAlbum" TypeName="WebMusic.Services.MusicLibrary">
                        <SelectParameters>
                            <asp:SessionParameter Name="album" SessionField="music.currentAlbum" 
                                Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </table>
                </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="all" EventName="ItemCommand" />
                </Triggers>
    </asp:UpdatePanel>
            </div>
        
     <script type="text/javascript">
    init();
    </script>



</asp:Content>
