using InboundLinkErrors.Core.Models;
using InboundLinkErrors.Core.Repositories;

namespace InboundLinkErrors.Core.Services
{
    public class LinkErrorsReferrerService
    {
        private readonly LinkErrorsReferrerRepository _repository;

        public LinkErrorsReferrerService(LinkErrorsReferrerRepository repository)
        {
            _repository = repository;
        }

        public void Add(LinkErrorReferrerDto model)
        {

        }
    }
}
