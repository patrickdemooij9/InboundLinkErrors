using System;
using System.Net;
using System.Web;
using InboundLinkErrors.Core.Processor;

namespace InboundLinkErrors.Core
{
    public class LinkErrorsInjectedModule : IHttpModule
    {
        private readonly ILinkErrorsProcessor _processor;

        public LinkErrorsInjectedModule(ILinkErrorsProcessor processor)
        {
            _processor = processor;
        }

        public void Init(HttpApplication context)
        {
            context.EndRequest += OnContextEndRequestHandler;
        }

        public void Dispose()
        {

        }

        private void OnContextEndRequestHandler(object sender, EventArgs args)
        {
            var application = (HttpApplication)sender;

            if (application.Response.StatusCode != (int)HttpStatusCode.NotFound) return;

            var request = application.Request;
            _processor.AddRequest(request.Url.AbsoluteUri, request.UrlReferrer?.AbsoluteUri, request.UserAgent);
        }
    }
}
