using System.Configuration;

namespace InboundLinkErrors.Core.ConfigurationProvider
{
    public class LinkErrorConfigurationProvider : ILinkErrorConfigurationProvider
    {
        private LinkErrorConfiguration _configuration;

        public LinkErrorConfiguration GetConfiguration()
        {
            return _configuration ?? (_configuration = new LinkErrorConfiguration
            {
                TrackUserAgents = GetBoolValue("InboundLinkErrors.TrackUserAgents") ?? true,
                TrackReferrer = GetBoolValue("InboundLinkErrors.TrackReferrer") ?? true,
                TrackMedia = GetBoolValue("InboundLinkErrors.TrackMedia") ?? true,
                SyncStartTime = GetIntValue("InboundLinkErrors.SyncStartupTime") ?? 60000,
                SyncInterval = GetIntValue("InboundLinkErrors.SyncInterval") ?? 300000,
                CleanupStartTime = GetIntValue("InboundLinkErrors.CleanupStartTime") ?? 600000,
                CleanupInterval = GetIntValue("InboundLinkErrors.CleanupInterval") ?? 14400000,
                CleanupAfterDays = GetIntValue("InboundLinkErrors.CleanupAfterDays") ?? 30
            });
        }

        private bool? GetBoolValue(string key)
        {
            if (bool.TryParse(ConfigurationManager.AppSettings[key], out var result))
                return result;
            return null;
        }

        private int? GetIntValue(string key)
        {
            if (int.TryParse(ConfigurationManager.AppSettings[key], out var result))
                return result;
            return null;
        }
    }
}
