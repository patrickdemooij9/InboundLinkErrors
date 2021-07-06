using System;
using System.IO;
using System.Net;
using System.Web;
using InboundLinkErrors.Core.ConfigurationProvider;
using InboundLinkErrors.Core.Processor;

namespace InboundLinkErrors.Core
{
    public class LinkErrorsInjectedModule : IHttpModule
    {
        private readonly ILinkErrorsProcessor _processor;
        private readonly LinkErrorConfiguration _configuration;

        public LinkErrorsInjectedModule(ILinkErrorsProcessor processor, ILinkErrorConfigurationProvider configurationProvider)
        {
            _processor = processor;
            _configuration = configurationProvider.GetConfiguration();
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

            if (!_configuration.TrackMedia && Path.HasExtension(request.Url.AbsoluteUri))
                return;

            var referrer = _configuration.TrackReferrer ? request.UrlReferrer?.AbsoluteUri : null;
            var userAgent = _configuration.TrackUserAgents ? request.UserAgent : null;

            _processor.AddRequest(request.Url.AbsoluteUri, referrer, userAgent);
        }
    }
}
