<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Page.Master"
Inherits="System.Web.Mvc.ViewPage<YearCalendarData>" %>

<%@ Import Namespace="Molecule.WebSite.atomes.photo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/atomes/photos/style/common.css" rel="stylesheet" type="text/css" />
    <link href="/atomes/photos/style/calendar.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <%--<photo:TagHierarchy ID="TagHierarchy" runat="server" TagQueryStringField="tag"/>--%>
    <br />
    <div class="BlockItem">
        <%--<asp:HyperLink ID="HyperLinkPrevious" runat="server">
            <img style="border:none" src="../../App_Themes/<%=Theme %>/images/go-previous.png" />
        </asp:HyperLink>--%>
    </div>
    <div class="BlockItem">
        <table style="border-collapse:collapse">
            <tbody>
            <% 2.Times().ForEach(i =>
               { %>
                <tr>
                <%Model.Items.Skip(i * 6).Take(6).ForEach((item, month) =>
              { %>
                        <td class="thinBorder" style="padding:0px">
                        <% Html.RenderPartial("PhotoLink",
                               new PhotoLinkData()
                               {
                                   Photo = item.Photo,
                                   Tag = Model.Tag,
                                   HoverText = item.Name,
                                   Description = item.Description,
                                   NavigateUrl = Url.Action("Month", "Calendar",
                                      new
                                      {
                                          year = Model.Year,
                                          month = month,
                                      }, null)
                               }); %>
                        </td>
                    <%}); %>
                </tr>
                <%}); %>
            </tbody>
        </table>
    </div>
    <div class="BlockItem">
        <%--<asp:HyperLink ID="HyperLinkNext" runat="server">
            <img style="border:none" src="../../App_Themes/<%=Theme %>/images/go-next.png" />
        </asp:HyperLink>--%>
    </div>
</asp:Content>
