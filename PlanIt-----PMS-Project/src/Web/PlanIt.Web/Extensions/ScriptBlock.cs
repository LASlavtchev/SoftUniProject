namespace PlanIt.Web.Extensions
{
    using System;
    using System.IO;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class ScriptBlock : IDisposable
    {
        private readonly ViewContext viewContext;
        private readonly TextWriter originalWriter;
        private readonly StringWriter scriptsWriter;

        public ScriptBlock(ViewContext viewContext)
        {
            this.viewContext = viewContext;
            this.originalWriter = viewContext.Writer;

            this.scriptsWriter = new StringWriter();
            this.viewContext.Writer = this.scriptsWriter;
        }

        public void Dispose()
        {
            this.viewContext.Writer = this.originalWriter;
            var pageScripts = HtmlExtensions.GetPageScriptsList(this.viewContext.HttpContext);
            pageScripts.Add(this.scriptsWriter.ToString());
        }
    }
}