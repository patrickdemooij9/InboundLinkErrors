using System;
using System.Runtime.InteropServices;
using InboundLinkErrors.Core.Services;
using Umbraco.Core.Logging;
using Umbraco.Web.Scheduling;

namespace InboundLinkErrors.Core.Tasks
{
    public class LinkErrorsDatabaseSyncTask : RecurringTaskBase
    {
        private readonly LinkErrorsService _linkErrorsService;
        private readonly ILogger _logger;

        public LinkErrorsDatabaseSyncTask(IBackgroundTaskRunner<RecurringTaskBase> runner,
            int delayBeforeWeStart,
            int howOftenWeRepeat,
            LinkErrorsService linkErrorsService, 
            ILogger logger) : base(runner, delayBeforeWeStart, howOftenWeRepeat)
        {
            _linkErrorsService = linkErrorsService;
            _logger = logger;
        }

        public override bool PerformRun()
        {
            try
            {
                _linkErrorsService.SyncToDatabase();
            }
            catch (Exception ex)
            {
                _logger.Error(typeof(LinkErrorsDatabaseSyncTask), ex);
                return false;
            }

            return true;
        }

        public override bool IsAsync => false;
    }
}
