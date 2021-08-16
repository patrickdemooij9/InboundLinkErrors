using System;
using System.Collections.Concurrent;
using System.Linq;
using InboundLinkErrors.Core.Interfaces;
using InboundLinkErrors.Core.Models.Dto;
using Umbraco.Extensions;

namespace InboundLinkErrors.Core.Processor
{
    public class LinkErrorsProcessor : ILinkErrorsProcessor
    {
        private readonly object _processorLock = new object();

        private readonly ILinkErrorsService _linkErrorsService;
        private ConcurrentBag<LinkErrorsProcessModel> _modelsToProcess;

        public LinkErrorsProcessor(ILinkErrorsService linkErrorsService)
        {
            _linkErrorsService = linkErrorsService;
            _modelsToProcess = new ConcurrentBag<LinkErrorsProcessModel>();
        }

        public void AddRequest(string requestUrl, string referrer = "", string userAgent = "")
        {
            var cleanedRequestUrl = requestUrl.TrimEnd('/');
            _modelsToProcess.Add(new LinkErrorsProcessModel(cleanedRequestUrl, referrer, userAgent));
        }

        public void ProcessData()
        {
            if (_modelsToProcess.Count == 0)
                return;

            //Swap the models so we can process them while we still collect new models for the next batch
            var models = _modelsToProcess;
            _modelsToProcess = new ConcurrentBag<LinkErrorsProcessModel>();
            //Interlocked.Exchange(ref models, _modelsToProcess);

            var groupedRequests = models.Where(it => !it.Deleted).GroupBy(it => it.RequestUrl).ToArray();
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

                foreach (var referrer in model.Select(it => it.Referrer).Distinct().WhereNotNull())
                {
                    var currentReferrer = linkErrorModel.Referrers.FirstOrDefault(it => it.Referrer.Equals(referrer, StringComparison.InvariantCultureIgnoreCase));
                    if (currentReferrer is null)
                    {
                        currentReferrer = new LinkErrorReferrerDto(referrer);
                        linkErrorModel.Referrers.Add(currentReferrer);
                    }
                    currentReferrer.LastAccessedTime = DateTime.UtcNow.Date;
                }

                foreach (var userAgent in model.Select(it => it.UserAgent).Distinct().WhereNotNull())
                {
                    var currentUserAgent = linkErrorModel.UserAgents.FirstOrDefault(it => it.UserAgent.Equals(userAgent, StringComparison.InvariantCultureIgnoreCase));
                    if (currentUserAgent is null)
                    {
                        currentUserAgent = new LinkErrorUserAgentDto(userAgent);
                        linkErrorModel.UserAgents.Add(currentUserAgent);
                    }
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
