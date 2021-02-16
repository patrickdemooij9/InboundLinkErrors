using InboundLinkErrors.Core.Interfaces;
using SimpleRedirects.Core;
using System.Net;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace InboundLinkErrors.SimpleRedirects.Core
{
    public class LinkErrorsSimpleRedirectAdapter : IRedirectAdapter
    {
        private readonly RedirectRepository _redirectRepository;

        public LinkErrorsSimpleRedirectAdapter(RedirectRepository redirectRepository)
        {
            _redirectRepository = redirectRepository;
        }

        public void AddRedirect(string fromUrl, IPublishedContent toNode, string culture)
        {
            _redirectRepository.AddRedirect(false, fromUrl, toNode.Url(culture), (int)HttpStatusCode.MovedPermanently, string.Empty);
        }
    }
}
