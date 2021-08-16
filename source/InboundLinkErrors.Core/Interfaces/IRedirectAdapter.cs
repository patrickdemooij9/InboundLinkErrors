using Umbraco.Cms.Core.Models.PublishedContent;

namespace InboundLinkErrors.Core.Interfaces
{
    public interface IRedirectAdapter
    {
        void AddRedirect(string fromUrl, IPublishedContent toNode, string culture);
    }
}
