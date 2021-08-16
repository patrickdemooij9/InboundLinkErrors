using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InboundLinkErrors.Core.Options
{
    public class LinkErrorsOptions
    {
        public const string Position = "InboundLinkErrors";

        public bool TrackReferrer { get; set; } = true;
        public bool TrackUserAgents { get; set; } = true;
        public bool TrackMedia { get; set; } = true;

        public int SyncStartTime { get; set; } = 60000;
        public int SyncInterval { get; set; } = 300000;

        public int CleanupStartTime { get; set; } = 600000;
        public int CleanupInterval { get; set; } = 14400000;
        public int CleanupAfterDays { get; set; } = 30;
    }
}
