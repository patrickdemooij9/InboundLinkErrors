﻿using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using InboundLinkErrors.Core.ConfigurationProvider;
using InboundLinkErrors.Core.Processor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;

namespace InboundLinkErrors.Core.Middleware
{
    public class LinkErrorsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILinkErrorsProcessor _processor;
        private readonly LinkErrorConfiguration _configuration;
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;

        public LinkErrorsMiddleware(RequestDelegate next, ILinkErrorsProcessor processor, ILinkErrorConfigurationProvider configurationProvider, IUmbracoContextAccessor umbracoContextAccessor)
        {
            _umbracoContextAccessor = umbracoContextAccessor;
            _next = next;
            _processor = processor;
            _configuration = configurationProvider.GetConfiguration();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var pathAndQuery = context.Request.GetEncodedPathAndQuery();

            if (pathAndQuery.IndexOf("/umbraco", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                await _next(context);
                return;
            }

            if (!_configuration.TrackMedia && Path.HasExtension(pathAndQuery))
            {
                await _next(context);
                return;
            }

            if (!_umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext))
            {
                await _next(context);
                return;
            }
            var publishedRequest = umbracoContext?.PublishedRequest;

            if (publishedRequest is not null && publishedRequest.ResponseStatusCode != StatusCodes.Status404NotFound)
            {
                await _next(context);
                return;
            }

            var headers = context.Request.GetTypedHeaders();
            var referrer = _configuration.TrackReferrer ? headers.Referer?.AbsoluteUri : null;
            var userAgent = _configuration.TrackUserAgents ? headers.Headers["User-Agent"].ToString() : null;

            _processor.AddRequest(context.Request.GetEncodedUrl(), referrer, userAgent);
            await _next(context);
        }
    }
}