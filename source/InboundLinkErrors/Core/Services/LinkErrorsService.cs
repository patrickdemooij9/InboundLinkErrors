using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using InboundLinkErrors.Core.Interfaces;
using InboundLinkErrors.Core.Models.Dto;
using InboundLinkErrors.Core.Repositories;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;

namespace InboundLinkErrors.Core.Services
{
    public class LinkErrorsService : ILinkErrorsService
    {
        private readonly object _linkErrorLock = new object();
        private readonly object _linkViewLock = new object();
        private readonly ConcurrentDictionary<string, LinkErrorDto> _itemsToSync;

        private readonly ILinkErrorsRepository _linkErrorsRepository;
        private readonly IRedirectAdapter _redirectService;

        public LinkErrorsService(ILinkErrorsRepository linkErrorsRepository, IRedirectAdapter redirectService)
        {
            _linkErrorsRepository = linkErrorsRepository;
            _redirectService = redirectService;

            _itemsToSync = new ConcurrentDictionary<string, LinkErrorDto>();
        }

        public LinkErrorDto Add(LinkErrorDto model)
        {
            return _linkErrorsRepository.Add(model);
        }

        public LinkErrorDto Update(LinkErrorDto model)
        {
            return _linkErrorsRepository.Update(model);
        }

        public void Delete(int id)
        {
            _linkErrorsRepository.Delete(_linkErrorsRepository.Get(id));
        }

        public LinkErrorDto Get(int id)
        {
            return _linkErrorsRepository.Get(id);
        }

        public IEnumerable<LinkErrorDto> GetByUrl(params string[] urls)
        {
            throw new NotImplementedException();
        }

        public LinkErrorDto GetByUrl(string url)
        {
            return _linkErrorsRepository.GetByUrl(url);
        }

        public IEnumerable<LinkErrorDto> GetAll()
        {
            return _linkErrorsRepository.GetAll();
        }

        public void ToggleHide(int id, bool toggle)
        {
            var dto = Get(id);
            dto.IsHidden = toggle;
            Update(dto);
        }

        public void TrackMissingLink(string requestUrl, string referrer = "", string userAgent = "")
        {
            var formattedUrl = requestUrl.ToLowerInvariant().TrimEnd("/");
            var linkError = GetLinkErrorByUrl(formattedUrl);
            var todayView = GetCurrentView(linkError);

            //TODO: Remove the IsDeleted logic. Just delete it
            //If a missing link is deleted, we want to "reset" it.
            /*if (linkError.IsDeleted)
            {
                todayView.VisitCount = 1;
                linkError.IsDeleted = false;
            }
            else
            {
                Interlocked.Increment(ref todayView.VisitCount);
            }*/

            todayView.Increment();

            //linkError.LastAccessedTime = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(referrer))
            {
                //_linkErrorsReferrerService.TrackReferrer(request.UrlReferrer.AbsoluteUri, linkError.Id);
            }

            if (!string.IsNullOrWhiteSpace(userAgent))
            {
                //_linkErrorsUserAgentService.TrackUserAgent(request.UserAgent, linkError.Id);
            }
        }

        public void SetRedirect(int linkErrorId, IPublishedContent nodeTo, string culture)
        {
            var linkError = Get(linkErrorId);
            _redirectService.AddRedirect(linkError.Url, nodeTo, culture);
            Delete(linkErrorId);
        }

        public void SyncToDatabase()
        {
            if (_itemsToSync.Count == 0)
                return;

            //TODO: Should we lock here?
            _linkErrorsRepository.UpdateOrAdd(_itemsToSync.Values);
            _itemsToSync.Clear();
        }

        private LinkErrorDto GetLinkErrorByUrl(string url)
        {
            if (_itemsToSync.ContainsKey(url))
                return _itemsToSync[url];

            lock (_linkErrorLock)
            {
                if (_itemsToSync.ContainsKey(url))
                    return _itemsToSync[url];

                var model = _linkErrorsRepository.GetByUrl(url) ?? new LinkErrorDto(url);

                _itemsToSync.TryAdd(url, model);
                return _itemsToSync[url];
            }
        }

        private LinkErrorViewDto GetCurrentView(LinkErrorDto model)
        {
            var currentView = model.Views.FirstOrDefault(it => it.Date == DateTime.UtcNow.Date);
            if (currentView != null)
                return currentView;

            lock (_linkViewLock)
            {
                currentView = model.Views.FirstOrDefault(it => it.Date == DateTime.UtcNow.Date);
                if (currentView != null)
                    return currentView;

                currentView = new LinkErrorViewDto(DateTime.UtcNow.Date);
                model.Views.Add(currentView);

                return currentView;
            }
        }
    }
}
