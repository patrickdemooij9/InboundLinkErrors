using System;
using System.Linq;
using System.Threading.Tasks;
using InboundLinkErrors.Core.ConfigurationProvider;
using InboundLinkErrors.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Infrastructure.HostedServices;

namespace InboundLinkErrors.Core.Tasks
{
    public class LinkErrorsCleanupTask : RecurringHostedServiceBase
    {
        private readonly int _daysToKeep;
        private readonly ILogger<LinkErrorsCleanupTask> _logger;
        private readonly ILinkErrorsService _linkErrorsService;

        public LinkErrorsCleanupTask(ILinkErrorConfigurationProvider configurationProvider, ILogger<LinkErrorsCleanupTask> logger, ILinkErrorsService linkErrorsService) : base(logger, configurationProvider.GetConfiguration().CleanupStartTime, configurationProvider.GetConfiguration().CleanupInterval)
        {
            _logger = logger;
            _linkErrorsService = linkErrorsService;
            _daysToKeep = configurationProvider.GetConfiguration().CleanupAfterDays;
        }

        public override Task PerformExecuteAsync(object state)
        {
            try
            {
                var today = DateTime.UtcNow.Date;
                var linkErrors = _linkErrorsService.GetAll().ToArray();
                foreach (var linkError in linkErrors)
                {
                    var dirty = false;
                    foreach (var view in linkError.Views.ToArray())
                    {
                        if (view.Date.AddDays(_daysToKeep) < today)
                        {
                            dirty = true;
                            linkError.Views.Remove(view);
                        }
                    }

                    if (!dirty) continue;

                    if (linkError.Views.Count == 0)
                        _linkErrorsService.Delete(linkError.Id);
                    else
                        _linkErrorsService.Update(linkError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong!");
            }

            return Task.CompletedTask;
        }
    }
}
