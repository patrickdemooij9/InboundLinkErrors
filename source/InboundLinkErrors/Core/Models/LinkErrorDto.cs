using System;
using System.IO;

namespace InboundLinkErrors.Core.Models
{
    public class LinkErrorDto
    {
        public int Id { get; set; }
        public string Url { get; }
        public bool IsHidden { get; set; }
        public bool IsDeleted { get; set; }
        public string LastReferrer { get; set; }
        public string LastUserAgent { get; set; }
        public int TimesAccessed { get; set; }
        public DateTime LastAccessedTime { get; set; }

        public bool IsMedia => Path.HasExtension(Url);

        public LinkErrorDto(string url)
        {
            Url = url;
        }
    }
}
