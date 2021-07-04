namespace InboundLinkErrors.Core.ConfigurationProvider
{
    public class LinkErrorConfiguration
    {
        public bool TrackReferrer { get; set; }
        public bool TrackUserAgents { get; set; }
        public bool TrackMedia { get; set; }
        
        public int SyncStartTime { get; set; }
        public int SyncInterval { get; set; }

        public int CleanupStartTime { get; set; }
        public int CleanupInterval { get; set; }
        public int CleanupAfterDays { get; set; }
    }
}
