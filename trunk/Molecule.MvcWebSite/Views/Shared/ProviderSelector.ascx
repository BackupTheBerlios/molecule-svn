<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ProviderSelectorData>" %>
<%foreach(var p in Model.Providers){ %> 
<%= Html.RadioButton("provider", p.Id, p.Id == Model.SelectedProviderId) + p.Name %>
<br />
<%} %>
<input type="submit" value="<%= Resources.Common.Reload %>" />