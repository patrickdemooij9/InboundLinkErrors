using System;
using System.Linq;
using InboundLinkErrors.Core.Interfaces;
using Umbraco.Core.Logging;
using Umbraco.Web.Scheduling;

namespace InboundLinkErrors.Core.Tasks
{
    public class LinkErrorsCleanupTask : RecurringTaskBase
    {
        private readonly int _daysToKeep;
        private readonly ILogger _logger;
        private readonly ILinkErrorsService _linkErrorsService;

        public LinkErrorsCleanupTask(IBackgroundTaskRunner<RecurringTaskBase> runner, int delayMilliseconds, int periodMilliseconds, int daysToKeep, ILogger logger, ILinkErrorsService linkErrorsService) : base(runner, delayMilliseconds, periodMilliseconds)
        {
            _daysToKeep = daysToKeep;
            _logger = logger;
            _linkErrorsService = linkErrorsService;
        }

        public override bool PerformRun()
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
                _logger.Error(typeof(LinkErrorsCleanupTask), ex);
                return false;
            }

            return true;
        }

        public override bool IsAsync => false;
    }
}
