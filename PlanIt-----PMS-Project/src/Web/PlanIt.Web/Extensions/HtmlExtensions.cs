namespace PlanIt.Web.Extensions
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class HtmlExtensions
    {
        private const string PartialViewScriptsKey = "_scripts_";

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
