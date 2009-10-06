<%@ Page Language="C#" MasterPageFile="~/Views/Shared/PreferencesPage.Master" Inherits="System.Web.Mvc.ViewPage<Molecule.MvcWebSite.atomes.photos.Data.AdminData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../../../Scripts/jquery-1.3.2.js"></script>
    <script type="text/javascript" src="../../../../Scripts/jquery.treeview.min.js"></script>
    <link rel="stylesheet" href='<%= Url.Theme("jquery.treeview.css") %>' type="text/css" />
    <script type="text/javascript">
        $(document).ready(function() {
            $(function() {
                $("#tagsTree").treeview({ collapsed: true, animated: "fast" });
            });
        });
    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="preferencesContent" runat="server">
    <% var form = Html.BeginForm("Save", "Admin", new { atome = "Photos" }, FormMethod.Post); %>
    <h2>Source</h2>
    <%= Resources.photo.GetPhotosFrom %>
    <br />
    <% Html.RenderPartial("ProviderSelector", Model); %>
    <br />
    <br />
    <%= Resources.photo.PhotoCollectionName %>
    <br />
    <% foreach (var tagName in Model.TagNames)
       { %>
    <%= Html.RadioButton("tagName", tagName.Key, Model.SelectedTagName.Key == tagName.Key) + tagName.Value%><br />
    <%} %>
    <h2><%= Model.SelectedTagName.Value %>s partagés</h2>
        
        <%= Html.TreeList(Model.RootTags, t => t.Tags,
        t => "<input " + (t.Selected ? "checked=\"checked\"" : "") + "value=" + t.Id + " type=\"checkbox\" name=\"sharedTags\" />" + t.Name
                            , new Dictionary<string, object>() { { "id", "tagsTree" } })%>
    <input type="submit" value="<%= Resources.Common.Save %>" />
    
    <h2><%=Resources.Common.Authorizations %></h2>
    <table>
        <thead>
            <tr>
                <td></td>
                <% foreach (var u in Model.UserNames)
                   { %><td>
                       <%= u%>
                </td>
                <%} %></tr>
        </thead>
        <tbody>
            <% foreach (var tua in Model.TagUserAuthorizations)
               { %>
            <tr>
                <td>
                    <%= tua.TagName%>
                </td>
                <%foreach (var auth in tua.Authorizations)
                  { %><td>
                      <input <%=auth.Authorized?"checked=\"checked\"":"" %> value="<%=auth.Value %>" type="checkbox"
                          name="authorizations" />
                  </td>
                <%} %></tr>
            <%} %>
        </tbody>
    </table>
       
    <%--
    <br />
    <asp:Button Text="<%$ Resources:Common,Save %>" runat="server" OnClick="save_onclick" />
    <br />
    <h2><asp:Literal runat="server" Text='<%$Resources:Common,AdvancedParameters %>' /></h2>
    
    <br />
   <asp:Literal runat="server" Text='<%$Resources:photo,ImageQuality %>' />: <asp:Label runat="server" ID="ImageQualityLabel" />%
    <asp:TextBox runat="server" id="ImageQualityTextBox" />
    <ajaxToolkit:SliderExtender ID="ImageQualityTextBox_SliderExtender" 
        runat="server" Enabled="True" Maximum="100" Minimum="0" 
        TargetControlID="ImageQualityTextBox" BoundControlID="ImageQualityLabel">
    </ajaxToolkit:SliderExtender>
    <br />
     <asp:Button runat="server" ID="EmptyCacheButton" 
        Text='<%$Resources:photo,ReinitCache %>' onclick="EmptyCacheButton_Click" />--%>
    <br />
    <input type="submit" value="<%= Resources.Common.Save %>" />
    <% form.EndForm(); %>
</asp:Content>
