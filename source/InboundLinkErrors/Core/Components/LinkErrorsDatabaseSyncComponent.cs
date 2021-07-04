using InboundLinkErrors.Core.ConfigurationProvider;
using InboundLinkErrors.Core.Processor;
using InboundLinkErrors.Core.Services;
using InboundLinkErrors.Core.Tasks;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Web.Scheduling;

namespace InboundLinkErrors.Core.Components
{
    public class LinkErrorsDatabaseSyncComponent : IComponent
    {
        private readonly ILinkErrorsProcessor _processor;
        private readonly ILinkErrorConfigurationProvider _configurationProvider;
        private readonly ILogger _logger;
        private readonly BackgroundTaskRunner<IBackgroundTask> _syncTask;

        public LinkErrorsDatabaseSyncComponent(ILogger logger, ILinkErrorsProcessor processor, ILinkErrorConfigurationProvider configurationProvider)
        {
            _processor = processor;
            _configurationProvider = configurationProvider;
            _logger = logger;

            _syncTask = new BackgroundTaskRunner<IBackgroundTask>("LinkErrorsDatabaseSync", logger);
        }

        public void Initialize()
        {
            var delayBeforeWeStart = _configurationProvider.GetConfiguration().SyncStartTime;
            var howOftenWeRepeat = _configurationProvider.GetConfiguration().SyncInterval;

            var task = new LinkErrorsDatabaseSyncTask(_syncTask, delayBeforeWeStart, howOftenWeRepeat, _processor, _logger);

            _syncTask.TryAdd(task);
        }

        public void Terminate()
        {
        }
    }
}
