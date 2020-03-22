namespace PlanIt.Web.Extensions
{
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    public static class HtmlExtensions
    {
        private const string PartialViewScriptsKey = "_scripts_";
        private static readonly HtmlContentBuilder EmptyBuilder = new HtmlContentBuilder();

        public static IHtmlContent BuildBreadcrumbNavigation(this IHtmlHelper helper)
        {
            var controllerName = helper.ViewContext.RouteData.Values["controller"].ToString();

            if (controllerName == "Home" || controllerName == "Account")
            {
                return EmptyBuilder;
            }

            var actionName = helper.ViewContext.RouteData.Values["action"].ToString();

            var breadcrumb = new HtmlContentBuilder()
                .AppendHtml("<ol class='breadcrumb float - sm - right'><li>")
                .AppendHtml(helper.ActionLink("Home", "Index", "Home"))
                .AppendHtml("</li><li>");


            return null;
        }

        public static IDisposable PartialSectionScripts(this IHtmlHelper helper)
        {
            var pageScript = new ScriptBlock(helper.ViewContext);

            return pageScript;
        }

        public static HtmlString RenderPartialSectionScripts(this IHtmlHelper helper)
        {
            var htmlString = new HtmlString(string.Join(
                 Environment.NewLine,
                 GetPageScriptsList(helper.ViewContext.HttpContext)));

            return htmlString;
        }

        public static List<string> GetPageScriptsList(HttpContext httpContext)
        {
            var pageScripts = (List<string>)httpContext.Items[PartialViewScriptsKey];

            if (pageScripts == null)
            {
                pageScripts = new List<string>();
                httpContext.Items[PartialViewScriptsKey] = pageScripts;
            }

            return pageScripts;
        }
    }
}
