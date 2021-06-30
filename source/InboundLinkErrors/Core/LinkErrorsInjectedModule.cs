using InboundLinkErrors.Core.Services;
using System;
using System.Net;
using System.Web;

namespace InboundLinkErrors.Core
{
    public class LinkErrorsInjectedModule : IHttpModule
    {
        private readonly LinkErrorsService _linkErrorsService;

        public LinkErrorsInjectedModule(LinkErrorsService linkErrorsService)
        {
            _linkErrorsService = linkErrorsService;
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
            _linkErrorsService.TrackMissingLink(request.Url.AbsoluteUri, request.UrlReferrer?.AbsoluteUri, request.UserAgent);
        }
    }
}
