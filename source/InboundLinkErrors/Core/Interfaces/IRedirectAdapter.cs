namespace InboundLinkErrors.Core.Interfaces
{
    public interface IRedirectAdapter
    {
        void AddRedirect(string fromUrl, string toUrl);
    }
}
