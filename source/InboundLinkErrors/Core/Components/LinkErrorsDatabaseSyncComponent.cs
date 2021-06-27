using InboundLinkErrors.Core.Services;
using InboundLinkErrors.Core.Tasks;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Web.Scheduling;

namespace InboundLinkErrors.Core.Components
{
    public class LinkErrorsDatabaseSyncComponent : IComponent
    {
        private readonly LinkErrorsService _linkErrorsService;
        private readonly ILogger _logger;
        private readonly BackgroundTaskRunner<IBackgroundTask> _syncTask;

        public LinkErrorsDatabaseSyncComponent(ILogger logger, LinkErrorsService linkErrorsService)
        {
            _linkErrorsService = linkErrorsService;
            _logger = logger;

            _syncTask = new BackgroundTaskRunner<IBackgroundTask>("LinkErrorsDatabaseSync", logger);
        }

        public void Initialize()
        {
            var delayBeforeWeStart = 60000; // 60000ms = 1min
            var howOftenWeRepeat = 300000; //300000ms = 5mins
            
            var task = new LinkErrorsDatabaseSyncTask(_syncTask, delayBeforeWeStart, howOftenWeRepeat, _linkErrorsService, _logger);

            _syncTask.TryAdd(task);
        }

        public void Terminate()
        {
        }
    }
}
