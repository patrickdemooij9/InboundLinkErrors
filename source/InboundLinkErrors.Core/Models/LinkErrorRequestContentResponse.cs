namespace InboundLinkErrors.Core.Models
{
    public class LinkErrorRequestContentResponse<T> : LinkErrorRequestResponse
    {
        public T Content { get; set; }
    }
}
