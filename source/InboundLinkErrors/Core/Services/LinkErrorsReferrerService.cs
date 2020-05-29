using System.Collections;
using System.Collections.Generic;
using InboundLinkErrors.Core.Models;
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

        public void Add(LinkErrorReferrerDto model, int linkErrorId)
        {
            var entity = _mapper.Map<LinkErrorReferrerDto, LinkErrorReferrerEntity>(model);
            entity.LinkErrorId = linkErrorId;

            _repository.Add(entity);
        }

        public void TrackReferrer(string referrer, int linkErrorId)
        {

        }

        public IEnumerable<LinkErrorReferrerDto> GetAll(int linkErrorId)
        {
            return _mapper.MapEnumerable<LinkErrorReferrerEntity, LinkErrorReferrerDto>(_repository.GetAllByLinkError(linkErrorId));
        }
    }
}
