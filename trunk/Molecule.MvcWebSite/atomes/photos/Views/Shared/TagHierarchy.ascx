<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TagHierarchyData>" %>
<%@ Import Namespace="System.Globalization" %>
<span style="font-size:120%">
<a href="<%= Url.Action<TagController>(c => c.Index(null), Atome.Id) %>">Photos</a>
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
   <%= Html.ActionLink<CalendarController>(Model.Year.ToString(), c => c.Year(Model.Year.Value,
        Model.Tag != null ? Model.Tag.Id : null), Atome.Id) %>
       <%if (Model.Month.HasValue)
         {%> > 
           <%= Html.ActionLink<CalendarController>(DateTimeFormatInfo.CurrentInfo.GetMonthName(Model.Month.Value),
               c => c.Month(Model.Year.Value, Model.Month.Value, Model.Tag != null ? Model.Tag.Id : null),
                   Atome.Id) %>
           
   <%}
   }%>
 </span>

