namespace InboundLinkErrors.Core.Models
{
    public class LinkErrorRequestResponse
    {
        public bool Success => string.IsNullOrWhiteSpace(ErrorMessage);
        public string ErrorMessage { get; }

        public LinkErrorRequestResponse(string errorMessage = "")
        {
            ErrorMessage = errorMessage;
        }
    }
}
