using System;

namespace InboundLinkErrors.Core.Models.Dto
{
    public class LinkErrorReferrerDto
    {
        public int Id { get; set; }
        public string Referrer { get; }
        public DateTime LastAccessedTime { get; set; }

        public LinkErrorReferrerDto(string referrer)
        {
            Referrer = referrer;
            LastAccessedTime = DateTime.UtcNow.Date;
        }
    }
}
