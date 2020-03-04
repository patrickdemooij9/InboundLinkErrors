namespace InboundLinkErrors.Core.Interfaces
{
    public interface IRedirectService
    {
        void AddRedirect(string fromUrl, string toUrl);
    }
}
