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
    public class LinkErrorsReferrerService
    {
        private readonly LinkErrorsReferrerRepository _repository;
        private readonly UmbracoMapper _mapper;

        public LinkErrorsReferrerService(LinkErrorsReferrerRepository repository, UmbracoMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public void TrackReferrer(string referrer, int linkErrorId)
        {
            var cleanedReferrer = referrer.ToLowerInvariant().Trim().TrimEnd('/');

            var entity = _repository.Get(linkErrorId, cleanedReferrer) ?? _repository.Add(new LinkErrorReferrerEntity { LinkErrorId = linkErrorId, Referrer = cleanedReferrer, LastAccessedTime = DateTime.UtcNow });

            entity.VisitCount++;
            entity.LastAccessedTime = DateTime.UtcNow;

            _repository.Update(entity);
        }

        public IEnumerable<LinkErrorReferrerDto> GetAll(int linkErrorId)
        {
            return _mapper.MapEnumerable<LinkErrorReferrerEntity, LinkErrorReferrerDto>(_repository.GetAllByLinkError(linkErrorId));
        }
    }
}
