using System;
using System.Collections.Generic;
using System.IO;

namespace InboundLinkErrors.Core.Models.Dto
{
    public class LinkErrorDto
    {
        public int Id { get; set; }
        public string Url { get; }
        public bool IsHidden { get; set; }
        public bool IsDeleted { get; set; }
        public int TimesAccessed { get; set; }
        public DateTime LastAccessedTime { get; set; }

        public Dictionary<string, int> Referrers { get; set; }
        public Dictionary<string, int> UserAgents { get; set; }

        public bool IsMedia => Path.HasExtension(Url);

        public LinkErrorDto(string url)
        {
            Url = url;
        }
    }
}