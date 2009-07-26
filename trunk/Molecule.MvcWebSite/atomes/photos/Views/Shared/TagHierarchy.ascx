<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TagHierarchyData>" %>
<%@ Import Namespace="System.Globalization" %>
<span style="font-size:120%">
<%= Html.RouteLink("Photos", "Tag", new { id = "" }) %>
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
       <%=Html.RouteLink(Model.Year.Value.ToString(),
           Model.Tag != null ? "TagYear" : "Year", new { year = Model.Year.Value })%> 
       <%if (Model.Month.HasValue)
         {%> > 
           <%=Html.RouteLink(DateTimeFormatInfo.CurrentInfo.GetMonthName(Model.Month.Value),
               Model.Tag != null ? "TagMonth" : "Month",
                          new { year = Model.Year.Value, month = Model.Month.Value })%>
   <%}
   }%>
 </span>

