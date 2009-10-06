<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Page.Master" Inherits="System.Web.Mvc.ViewPage<MonthCalendarData>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/atomes/photos/style/common.css" rel="stylesheet" type="text/css" />
    <link href="/atomes/photos/style/calendar.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <% Html.RenderPartial("TagHierarchy", new TagHierarchyData() { Tag = Model.Tag, Month = Model.Month, Year = Model.Year }); %>
    <br />
    <div class="BlockItem">
     <a href="<%= CalendarController.MonthUrl(Url, Model.PreviousYear, Model.PreviousMonth, Model.Tag) %>">
            <img style="border:none" src="<%= Url.Theme("images/go-previous.png")%>" />
        </a>
    </div>
    <div class="BlockItem">
    <table style="border-collapse:collapse;padding:0px;margin:0px">
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
                    <td class='<%= item.IsEmpty ?"":"thinBorder" %>' style="padding:0px">
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
    </div>
    <div class="BlockItem">
    <a href="<%= CalendarController.MonthUrl(Url, Model.NextYear, Model.NextMonth, Model.Tag) %>">
            <img style="border:none" src="<%= Url.Theme("images/go-next.png")%>" />
        </a>
    </div>
</asp:Content>
