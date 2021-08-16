using System;
using InboundLinkErrors.Core.Options;
using Microsoft.Extensions.Options;

namespace InboundLinkErrors.Core.ConfigurationProvider
{
    public class LinkErrorConfigurationProvider : ILinkErrorConfigurationProvider
    {
        private readonly IOptions<LinkErrorsOptions> _options;
        private LinkErrorConfiguration _configuration;

        public LinkErrorConfigurationProvider(IOptions<LinkErrorsOptions> options)
        {
            _options = options;
        }

        public LinkErrorConfiguration GetConfiguration()
        {
            if (_configuration != null)
                return _configuration;

            var config = _options.Value;
            _configuration = new LinkErrorConfiguration
            {
                TrackUserAgents = config.TrackUserAgents,
                TrackReferrer = config.TrackReferrer,
                TrackMedia = config.TrackMedia,
                SyncStartTime = TimeSpan.FromMilliseconds(config.SyncStartTime),
                SyncInterval = TimeSpan.FromMilliseconds(config.SyncInterval),
                CleanupStartTime = TimeSpan.FromMilliseconds(config.CleanupStartTime),
                CleanupInterval = TimeSpan.FromMilliseconds(config.CleanupInterval),
                CleanupAfterDays = config.CleanupAfterDays
            };
            return _configuration;
        }
    }
}
