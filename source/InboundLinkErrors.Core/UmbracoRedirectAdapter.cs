using System;
using InboundLinkErrors.Core.Interfaces;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;

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
