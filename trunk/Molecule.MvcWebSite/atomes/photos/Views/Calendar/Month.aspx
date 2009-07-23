<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Page.Master"
Inherits="System.Web.Mvc.ViewPage<MonthCalendarData>" %>
<%@ Import Namespace="System.Globalization" %>

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
       <%-- <asp:HyperLink ID="HyperLinkNext" runat="server">
            <img style="border:none" src="../../App_Themes/<%=Theme %>/images/go-next.png" />
        </asp:HyperLink>--%>
    </div>
</asp:Content>
