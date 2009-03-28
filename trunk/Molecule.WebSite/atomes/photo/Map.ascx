<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Map.ascx.cs" Inherits="Molecule.WebSite.atomes.photo.Map" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:ScriptManagerProxy ID="ScriptManagerProxy" runat="server">
    <Scripts>
        <asp:ScriptReference Path="http://www.openlayers.org/api/OpenLayers.js" />
        <asp:ScriptReference Path="http://www.openstreetmap.org/openlayers/OpenStreetMap.js" />
        <asp:ScriptReference Path="scripts/osm.js" />
    </Scripts>
</asp:ScriptManagerProxy>
<div>
     <asp:LinkButton ID="LocateLink" runat="server">Locate!</asp:LinkButton>
     <div id="mapPopup" class="popup">
        <div class="popupHeader"><span class="left">Map by <a href="http://openstreetmap.org">OpenStreetMap</a></span>&nbsp;<span id="closeModal" class="right"><asp:LinkButton ID="CloseButton" runat="server">close</asp:LinkButton></span></div>
        <div id="map">
        </div>
        <script type="text/javascript">
loadMap('map', <%=Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture) %>, <%=Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture) %>, '<%= ThumbnailUrl %>');
</script>
     </div>
    <ajaxToolkit:ModalPopupExtender ID="MPE" runat="server"
    TargetControlID="LocateLink"
    PopupControlID="mapPopup" 
    BackgroundCssClass="modalBackground"
    DropShadow="true" 
    CancelControlID="closeModal" />
</div>
