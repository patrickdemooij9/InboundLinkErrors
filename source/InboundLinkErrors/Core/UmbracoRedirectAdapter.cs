using System;
using InboundLinkErrors.Core.Interfaces;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace InboundLinkErrors.Core
{
    public class UmbracoRedirectAdapter : IRedirectAdapter
    {
        private readonly IUmbracoContextFactory _contextFactory;
        private readonly IRedirectUrlService _redirectUrlService;

        public UmbracoRedirectAdapter(IUmbracoContextFactory contextFactory, IRedirectUrlService redirectUrlService)
        {
            _contextFactory = contextFactory;
            _redirectUrlService = redirectUrlService;
        }

        public void AddRedirect(string fromUrl, IPublishedContent nodeTo, string culture)
        {
            _redirectUrlService.Register(new Uri(fromUrl).PathAndQuery.ToLowerInvariant(), nodeTo.Key, culture);
        }
    }
}
