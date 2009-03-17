<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Map.ascx.cs" Inherits="Molecule.WebSite.atomes.photo.Map" %>
<asp:ScriptManagerProxy ID="ScriptManagerProxy" runat="server">
    <Scripts>
        <asp:ScriptReference Path="http://www.openlayers.org/api/OpenLayers.js" />
        <asp:ScriptReference Path="http://www.openstreetmap.org/openlayers/OpenStreetMap.js" />
        <asp:ScriptReference Path="scripts/osm.js" />
    </Scripts>
</asp:ScriptManagerProxy>
<div id="map">
</div>
<script type="text/javascript">
loadMap('map', <%=Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture) %>, <%=Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture) %>);
</script>
