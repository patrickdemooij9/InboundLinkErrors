using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InboundLinkErrors.Core.Models.Data;

namespace InboundLinkErrors.Core.Models.Dto
{
    public class LinkErrorDto
    {
        public int Id { get; set; }
        public string Url { get; }
        public bool IsHidden { get; set; }
        public bool IsDeleted { get; set; }

        public List<LinkErrorReferrerDto> Referrers { get; set; }
        public List<LinkErrorUserAgentDto> UserAgents { get; set; }
        public List<LinkErrorViewDto> Views { get; set; }

        public bool IsMedia => Path.HasExtension(Url);
        public int TotalVisits => Views.Sum(it => it.VisitCount);

        public LinkErrorDto(string url)
        {
            Url = url;
            Referrers = new List<LinkErrorReferrerDto>();
            UserAgents = new List<LinkErrorUserAgentDto>();
            Views = new List<LinkErrorViewDto>();
        }
    }
}
