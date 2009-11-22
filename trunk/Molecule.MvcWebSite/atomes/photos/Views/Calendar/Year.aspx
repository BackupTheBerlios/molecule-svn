<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Page.Master"
Inherits="System.Web.Mvc.ViewPage<YearCalendarData>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/atomes/photos/style/common.css" rel="stylesheet" type="text/css" />
    <link href="/atomes/photos/style/calendar.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <% Html.RenderPartial("TagHierarchy", new TagHierarchyData() { Tag = Model.Tag, Year = Model.Year }); %>
    <br />
        <% using(Html.ActionLink<CalendarController>(c => c.Year(Model.PreviousYear,
               Model.Tag.NotNull(t => t.Id)))){ %>
            <img style="border:none" src="<%= Url.Theme("images/go-previous.png")%>" />
        <%} %>
        <% using (Html.ActionLink<CalendarController>(c => c.Year(Model.NextYear,
               Model.Tag.NotNull(t => t.Id)))) { %>
            <img style="border:none" src="<%= Url.Theme("images/go-next.png")%>" />
        <%} %>
        <table style="calendarTable">
            <tbody>
            <% 2.Times().ForEach(i =>
               { %>
                <tr>
                <%Model.Items.Skip(i * 6).Take(6).ForEach((item, m) =>
              { %>
                        <td class="thinBorder calendarItem">
                        <% Html.RenderPartial("PhotoLink",
                               new PhotoLinkData()
                               {
                                   Photo = item.Photo,
                                   Tag = Model.Tag,
                                   HoverText = item.Name,
                                   Description = item.Description,
                                   NavigateUrl = Url.Action<CalendarController>(c =>
                                       c.Month(Model.Year, i * 6 + m + 1, Model.Tag.NotNull(t => t.Id)))
                                     
                               }); %>
                        </td>
                    <%}); %>
                </tr>
                <%}); %>
            </tbody>
        </table>

        

</asp:Content>
