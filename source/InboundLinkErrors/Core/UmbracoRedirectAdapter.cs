using System;
using InboundLinkErrors.Core.Interfaces;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace InboundLinkErrors.Core
{
    public class UmbracoRedirectAdapter : IRedirectService
    {
        private readonly IUmbracoContextFactory _contextFactory;
        private readonly IRedirectUrlService _redirectUrlService;

        public UmbracoRedirectAdapter(IUmbracoContextFactory contextFactory, IRedirectUrlService redirectUrlService)
        {
            _contextFactory = contextFactory;
            _redirectUrlService = redirectUrlService;
        }

        public void AddRedirect(string fromUrl, string toUrl)
        {
            using (var contextRef = _contextFactory.EnsureUmbracoContext())
            {
                _redirectUrlService.Register(new Uri(fromUrl).PathAndQuery.ToLowerInvariant(), contextRef.UmbracoContext.Content.GetByRoute(toUrl).Key);
            }
        }
    }
}
