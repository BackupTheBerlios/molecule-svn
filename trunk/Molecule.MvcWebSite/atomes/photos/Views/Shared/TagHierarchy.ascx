<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TagHierarchyData>" %>
<%@ Import Namespace="System.Globalization" %>
<span style="font-size:120%">
<a href="<%= TagController.IndexUrl(Url, null)%>">Photos</a>
 <%if (Model.Tag != null)
   {
       foreach (var tag in PhotoLibrary.GetTagHierarchy(Model.Tag.Id))
       {
           Writer.Write(" > ");
           Html.RenderPartial("TagLink", new TagLinkData() { Tag = tag, TextOnly = true });
       }
   }
   if (Model.Year.HasValue)
   {%> > 
       <a href="<%= CalendarController.YearUrl(Url, Model.Year.Value, Model.Tag)%> "><%= Model.Year %></a>
       <%if (Model.Month.HasValue)
         {%> > 
           <a href="<%=CalendarController.MonthUrl(Url, Model.Year.Value, Model.Month.Value, Model.Tag)%>">
           <%= DateTimeFormatInfo.CurrentInfo.GetMonthName(Model.Month.Value)%></a>
   <%}
   }%>
 </span>

