﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Page.Master"
Inherits="System.Web.Mvc.ViewPage<YearCalendarData>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/atomes/photos/style/common.css" rel="stylesheet" type="text/css" />
    <link href="/atomes/photos/style/calendar.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <% Html.RenderPartial("TagHierarchy", new TagHierarchyData() { Tag = Model.Tag, Year = Model.Year }); %>
    <br />
    <div class="BlockItem">
        <a href='<%= Url.Action<CalendarController>(c => c.Year(Model.PreviousYear, Model.Tag != null ? Model.Tag.Id : null)) %>'>
            <img style="border:none" src="<%= Url.Theme("images/go-previous.png")%>" />
        </a>
    </div>
    <div class="BlockItem">
        <table style="border-collapse:collapse">
            <tbody>
            <% 2.Times().ForEach(i =>
               { %>
                <tr>
                <%Model.Items.Skip(i * 6).Take(6).ForEach((item, m) =>
              { %>
                        <td class="thinBorder" style="padding:0px">
                        <% Html.RenderPartial("PhotoLink",
                               new PhotoLinkData()
                               {
                                   Photo = item.Photo,
                                   Tag = Model.Tag,
                                   HoverText = item.Name,
                                   Description = item.Description,
                                   NavigateUrl = Url.RouteUrl("TagMonth",
                                      new
                                      {
                                          tagId = Model.Tag.Id,
                                          year = Model.Year,
                                          month = i*6+m+1,
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
        <a href="<%= Url.Action<CalendarController>(c => c.Year(Model.NextYear, Model.Tag != null ? Model.Tag.Id : null)) %>">
            <img style="border:none" src="<%= Url.Theme("images/go-next.png")%>" />
        </a>
    </div>
</asp:Content>
