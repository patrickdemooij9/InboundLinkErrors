using InboundLinkErrors.Core.ConfigurationProvider;
using InboundLinkErrors.Core.Interfaces;
using InboundLinkErrors.Core.Tasks;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Web.Scheduling;

namespace InboundLinkErrors.Core.Components
{
    public class LinkErrorsCleanupComponent : IComponent
    {
        private readonly ILogger _logger;
        private readonly ILinkErrorConfigurationProvider _configurationProvider;
        private readonly ILinkErrorsService _linkErrorsService;
        private readonly BackgroundTaskRunner<IBackgroundTask> _task;

        public LinkErrorsCleanupComponent(ILogger logger, ILinkErrorConfigurationProvider configurationProvider, ILinkErrorsService linkErrorsService)
        {
            _logger = logger;
            _configurationProvider = configurationProvider;
            _linkErrorsService = linkErrorsService;

            _task = new BackgroundTaskRunner<IBackgroundTask>("LinkErrorsCleanup", logger);
        }

        public void Initialize()
        {
            var configuration = _configurationProvider.GetConfiguration();
            var delayBeforeWeStart = configuration.CleanupStartTime;
            var howOftenWeRepeat = configuration.CleanupInterval;

            var task = new LinkErrorsCleanupTask(_task, delayBeforeWeStart, howOftenWeRepeat, configuration.CleanupAfterDays, _logger, _linkErrorsService);

            _task.TryAdd(task);
        }

        public void Terminate()
        {
        }
    }
}
