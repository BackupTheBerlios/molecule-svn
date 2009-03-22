<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FullSizePhoto.ascx.cs"
    Inherits="Molecule.WebSite.atomes.photo.FullSizePhoto" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="WebPhoto.Services" %>
<%@ Import Namespace="Molecule.WebSite.atomes.photo" %>

<div id="photo">
    <asp:Panel id="metadatas" CssClass="metadatas" runat="server">
        <asp:GridView ID="MetadatasGridView" runat="server" AutoGenerateColumns="true" ShowHeader="false">
        </asp:GridView>
    </asp:Panel>
    <asp:Image runat="server" ID="image" />
        <ajaxToolkit:AnimationExtender ID="ae"
  runat="server" TargetControlID="image">
    <Animations>
        <OnLoad>
            <FadeIn minimumOpacity="0" maximumOpacity="1" Duration=".3" Fps="20" />
        </OnLoad>
        
        <OnHoverOver>
            <FadeIn AnimationTarget="metadatas" minimumOpacity="0" maximumOpacity="0.7" Duration=".3" Fps="20" />
        </OnHoverOver>
        
        <OnHoverOut>
            <FadeOut AnimationTarget="metadatas" minimumOpacity="0" maximumOpacity="0.7" Duration=".3" Fps="20" />
        </OnHoverOut>
        

    </Animations>
</ajaxToolkit:AnimationExtender>
</div>
