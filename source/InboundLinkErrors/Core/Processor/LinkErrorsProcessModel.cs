namespace InboundLinkErrors.Core.Processor
{
    public class LinkErrorsProcessModel
    {
        public string RequestUrl { get; }
        public string Referrer { get; }
        public string UserAgent { get; }
        public bool Deleted { get; set; }

        public LinkErrorsProcessModel(string requestUrl, string referrer = "", string userAgent = "")
        {
            RequestUrl = requestUrl;
            Referrer = referrer;
            UserAgent = userAgent;
        }
    }
}
