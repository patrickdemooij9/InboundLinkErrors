namespace InboundLinkErrors.Core.Processor
{
    public interface ILinkErrorsProcessor
    {
        void AddRequest(string requestUrl, string referrer = "", string userAgent = "");
        void ProcessData();
    }
}
