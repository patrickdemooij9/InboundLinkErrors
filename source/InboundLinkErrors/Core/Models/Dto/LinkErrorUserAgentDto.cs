namespace InboundLinkErrors.Core.Models.Dto
{
    public class LinkErrorUserAgentDto
    {
        public int Id { get; set; }
        public string UserAgent { get; set; }
        public int VisitCount { get; set; }
    }
}
