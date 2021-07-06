using System;
using System.Runtime.InteropServices;
using InboundLinkErrors.Core.Processor;
using InboundLinkErrors.Core.Services;
using Umbraco.Core.Logging;
using Umbraco.Web.Scheduling;

namespace InboundLinkErrors.Core.Tasks
{
    public class LinkErrorsDatabaseSyncTask : RecurringTaskBase
    {
        private readonly ILinkErrorsProcessor _processor;
        private readonly ILogger _logger;

        public LinkErrorsDatabaseSyncTask(IBackgroundTaskRunner<RecurringTaskBase> runner,
            int delayBeforeWeStart,
            int howOftenWeRepeat,
            ILinkErrorsProcessor processor, 
            ILogger logger) : base(runner, delayBeforeWeStart, howOftenWeRepeat)
        {
            _processor = processor;
            _logger = logger;
        }

        public override bool PerformRun()
        {
            try
            {
                _processor.ProcessData();
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
