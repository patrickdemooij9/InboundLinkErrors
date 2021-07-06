using System;

namespace InboundLinkErrors.Core.Models.Dto
{
    public class LinkErrorUserAgentDto
    {
        public int Id { get; set; }
        public string UserAgent { get; set; }
        public DateTime LastAccessedTime { get; set; }

        public LinkErrorUserAgentDto(string userAgent)
        {
            UserAgent = userAgent;
            LastAccessedTime = DateTime.UtcNow.Date;
        }
    }
}
