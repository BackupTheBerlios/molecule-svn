<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ProviderSelectorData>" %>
<% var form = Html.BeginForm();
   var providerItems = from p in Model.Providers select new SelectListItem() { Text = p.Name, Value = p.Id }; %> 
<%= Html.DropDownList("providerSelector", providerItems)%>
<input type="submit" value="<%= Resources.Common.Reload %>" />
<% form.EndForm(); %>