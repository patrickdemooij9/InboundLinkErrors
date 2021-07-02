using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using InboundLinkErrors.Core.Interfaces;
using InboundLinkErrors.Core.Models.Dto;

namespace InboundLinkErrors.Core.Processor
{
    public class LinkErrorsProcessor : ILinkErrorsProcessor
    {
        private readonly object _processorLock = new object();

        private readonly ILinkErrorsService _linkErrorsService;
        private readonly ConcurrentBag<LinkErrorsProcessModel> _modelsToProcess;

        public LinkErrorsProcessor(ILinkErrorsService linkErrorsService)
        {
            _linkErrorsService = linkErrorsService;
            _modelsToProcess = new ConcurrentBag<LinkErrorsProcessModel>();
        }

        public void AddRequest(string requestUrl, string referrer = "", string userAgent = "")
        {
            _modelsToProcess.Add(new LinkErrorsProcessModel(requestUrl.TrimEnd('/'), referrer, userAgent));
        }

        public void ProcessData()
        {
            //Swap the models so we can process them while we still collect new models for the next batch
            var models = new ConcurrentBag<LinkErrorsProcessModel>();
            Interlocked.Exchange(ref models, _modelsToProcess);

            var groupedRequests = models.GroupBy(it => it.RequestUrl).ToArray();
            var linkErrorModels = _linkErrorsService.GetByUrl(groupedRequests.Select(it => it.Key).ToArray()).ToDictionary(it => it.Url, it => it);
            foreach (var model in groupedRequests)
            {
                var exists = linkErrorModels.ContainsKey(model.Key);
                var newViews = model.Count();
                var linkErrorModel = exists ? linkErrorModels[model.Key] : new LinkErrorDto(model.Key);

                var currentView = linkErrorModel.Views.FirstOrDefault(it => it.Date == DateTime.UtcNow.Date);
                if (currentView is null)
                {
                    currentView = new LinkErrorViewDto(DateTime.UtcNow.Date);
                    linkErrorModel.Views.Add(currentView);
                }
                currentView.VisitCount += newViews;

                foreach (var referrer in model.Select(it => it.Referrer).Distinct())
                {
                    var currentReferrer = linkErrorModel.Referrers.FirstOrDefault(it => it.Referrer.Equals(referrer, StringComparison.InvariantCultureIgnoreCase)) ?? new LinkErrorReferrerDto(referrer);
                    currentReferrer.LastAccessedTime = DateTime.UtcNow.Date;
                }

                foreach (var userAgent in model.Select(it => it.UserAgent).Distinct())
                {
                    var currentUserAgent = linkErrorModel.UserAgents.FirstOrDefault(it => it.UserAgent.Equals(userAgent, StringComparison.InvariantCultureIgnoreCase)) ?? new LinkErrorUserAgentDto(userAgent);
                    currentUserAgent.LastAccessedTime = DateTime.UtcNow.Date;
                }

                if (exists)
                    _linkErrorsService.Update(linkErrorModel);
                else
                    _linkErrorsService.Add(linkErrorModel);
            }
        }
    }
}
