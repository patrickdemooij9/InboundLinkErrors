using System;
using Newtonsoft.Json;

namespace InboundLinkErrors.Core.Models.ViewModels
{
    public class LinkErrorViewModel
    {
        [JsonProperty("items")]
        public LinkErrorItemViewModel[] Items { get; set; }

        [JsonProperty("allowMedia")]
        public bool AllowMedia { get; set; }

        public LinkErrorViewModel()
        {
            Items = Array.Empty<LinkErrorItemViewModel>();
        }
    }
}
