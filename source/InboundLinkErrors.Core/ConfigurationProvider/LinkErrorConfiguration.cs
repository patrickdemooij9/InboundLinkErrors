using System;

namespace InboundLinkErrors.Core.ConfigurationProvider
{
    public class LinkErrorConfiguration
    {
        public bool TrackReferrer { get; set; }
        public bool TrackUserAgents { get; set; }
        public bool TrackMedia { get; set; }
        
        public TimeSpan SyncStartTime { get; set; }
        public TimeSpan SyncInterval { get; set; }

        public TimeSpan CleanupStartTime { get; set; }
        public TimeSpan CleanupInterval { get; set; }
        public int CleanupAfterDays { get; set; }
    }
}
