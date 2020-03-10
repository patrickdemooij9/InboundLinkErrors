using InboundLinkErrors.Core.Interfaces;
using SimpleRedirects.Core;
using System.Net;

namespace InboundLinkErrors.SimpleRedirects.Core
{
    public class LinkErrorsSimpleRedirectAdapter : IRedirectAdapter
    {
        private readonly RedirectRepository _redirectRepository;

        public LinkErrorsSimpleRedirectAdapter(RedirectRepository redirectRepository)
        {
            _redirectRepository = redirectRepository;
        }

        public void AddRedirect(string fromUrl, string toUrl)
        {
            _redirectRepository.AddRedirect(false, fromUrl, toUrl, (int)HttpStatusCode.MovedPermanently, string.Empty);
        }
    }
}
