using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace InboundLinkErrors.Core.Models.ViewModels
{
    public class LinkErrorItemViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("relativeUrl")]
        public string RelativeUrl { get; set; }

        [JsonProperty("isHidden")]
        public bool IsHidden { get; set; }

        [JsonProperty("isMedia")]
        public bool IsMedia { get; set; }

        [JsonProperty("views")]
        public Dictionary<DateTime, int> Views { get; set; }
        [JsonProperty("referrers")]
        public string[] Referrers { get; set; }
        [JsonProperty("userAgents")]
        public string[] UserAgents { get; set; }

        [JsonProperty("totalVisits")]
        public int TotalVisits => Views.Sum(it => it.Value);
        [JsonProperty("lastAccessed")]
        public DateTime LastAccessed => Views.Keys.Count == 0 ? DateTime.MinValue : Views.Keys.Max();

        public LinkErrorItemViewModel()
        {
            Views = new Dictionary<DateTime, int>();
            Referrers = Array.Empty<string>();
            UserAgents = Array.Empty<string>();
        }
    }
}
