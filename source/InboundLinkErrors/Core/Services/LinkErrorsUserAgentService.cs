using System;
using System.Collections;
using System.Collections.Generic;
using InboundLinkErrors.Core.Models;
using InboundLinkErrors.Core.Models.Data;
using InboundLinkErrors.Core.Models.Dto;
using InboundLinkErrors.Core.Repositories;
using Umbraco.Core.Mapping;

namespace InboundLinkErrors.Core.Services
{
    public class LinkErrorsUserAgentService
    {
        private readonly LinkErrorsUserAgentRepository _repository;
        private readonly UmbracoMapper _mapper;

        public LinkErrorsUserAgentService(LinkErrorsUserAgentRepository repository, UmbracoMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public void TrackUserAgent(string userAgent, int linkErrorId)
        {
            var cleanedUserAgent = userAgent.ToLowerInvariant().Trim();
            var entity = _repository.Get(linkErrorId, userAgent) ?? _repository.Add(new LinkErrorUserAgentEntity
            {
                LinkErrorId = linkErrorId, UserAgent = cleanedUserAgent, LastAccessedTime = DateTime.UtcNow
            });

            entity.VisitCount++;
            entity.LastAccessedTime = DateTime.UtcNow;

            _repository.Update(entity);
        }

        public IEnumerable<LinkErrorUserAgentDto> GetAll(int linkErrorId)
        {
            return _mapper.MapEnumerable<LinkErrorUserAgentEntity, LinkErrorUserAgentDto>(_repository.GetAllByLinkError(linkErrorId));
        }
    }
}
