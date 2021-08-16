using System;
using System.Threading.Tasks;
using InboundLinkErrors.Core.ConfigurationProvider;
using InboundLinkErrors.Core.Processor;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Infrastructure.HostedServices;

namespace InboundLinkErrors.Core.Tasks
{
    public class LinkErrorsDatabaseSyncTask : RecurringHostedServiceBase
    {
        private readonly ILinkErrorsProcessor _processor;
        private readonly ILogger<LinkErrorsDatabaseSyncTask> _logger;

        public LinkErrorsDatabaseSyncTask(ILinkErrorConfigurationProvider configurationProvider, ILinkErrorsProcessor processor, ILogger<LinkErrorsDatabaseSyncTask> logger) : base(configurationProvider.GetConfiguration().SyncStartTime, configurationProvider.GetConfiguration().SyncInterval)
        {
            _processor = processor;
            _logger = logger;
        }

        public override Task PerformExecuteAsync(object state)
        {
            try
            {
                _processor.ProcessData();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong!");
            }

            return Task.CompletedTask;
        }
    }
}
