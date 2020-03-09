using InboundLinkErrors.Core.Interfaces;
using SimpleRedirects.Core;
using System.Net;

namespace InboundLinkErrors.SimpleRedirects.Core
{
    public class MissingLinkSimpleRedirectAdapter : IRedirectAdapter
    {
        private readonly RedirectRepository _redirectRepository;

        public MissingLinkSimpleRedirectAdapter(RedirectRepository redirectRepository)
        {
            _redirectRepository = redirectRepository;
        }

        public void AddRedirect(string fromUrl, string toUrl)
        {
            _redirectRepository.AddRedirect(false, fromUrl, toUrl, (int)HttpStatusCode.MovedPermanently, string.Empty);
        }
    }
}
