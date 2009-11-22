<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Page.Master" Inherits="System.Web.Mvc.ViewPage<MonthCalendarData>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/atomes/photos/style/common.css" rel="stylesheet" type="text/css" />
    <link href="/atomes/photos/style/calendar.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
   
    <% Html.RenderPartial("TagHierarchy", new TagHierarchyData() { Tag = Model.Tag, Month = Model.Month, Year = Model.Year }); %>
      <br />
      <% using (Html.ActionLink<CalendarController>(c => c.Month(Model.PreviousYear,
            Model.PreviousMonth, Model.Tag.NotNull(t => t.Id)))) { %>
            <img style="border:none" src="<%= Url.Theme("images/go-previous.png")%>" />
        <%} %>
     <% using (Html.ActionLink<CalendarController>(c =>
           c.Month(Model.NextYear, Model.NextMonth, Model.Tag.NotNull(t => t.Id)))) { %>
            <img style="border:none" src="<%= Url.Theme("images/go-next.png")%>" />
        <%} %>

    <table class="calendarTable">
        <thead>
            <tr>
            <%for (int i = 0; i < 7; i++)
              { %>
                <td><%= DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(
                        (DayOfWeek)(((int)DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek + i) % 7))%></td>
            <%} %>
            </tr>
        </thead>
            <tbody>
            <%for (int i = 0; i < 6; i++)
              { %>
                <tr>
                <% Model.Items.Skip(i * 7).Take(7).ForEach(item =>
                      { %>
                    <td class='<%= item.IsEmpty ?"":"thinBorder calendarItem" %>'>
                    <% Html.RenderPartial("PhotoLink",
                               new PhotoLinkData()
                               {
                                   Photo = item.Photo,
                                   Tag = Model.Tag,
                                   HoverText = item.Name,
                                   Description = item.Description
                               }); %>
                    </td>
                <%}); %>
                </tr>
                <%} %>
            </tbody>
        </table>
   
</asp:Content>
