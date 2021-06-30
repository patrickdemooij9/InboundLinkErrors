using System.Collections.Generic;
using InboundLinkErrors.Core.Interfaces;
using InboundLinkErrors.Core.Models.Data;
using InboundLinkErrors.Core.Models.Dto;
using NPoco;
using Umbraco.Core.Mapping;
using Umbraco.Core.Persistence;
using Umbraco.Core.Scoping;

namespace InboundLinkErrors.Core.Repositories
{
    public class LinkErrorsRepository : ILinkErrorsRepository
    {
        private readonly IScopeProvider _scopeProvider;
        private readonly UmbracoMapper _umbracoMapper;

        public LinkErrorsRepository(IScopeProvider scopeProvider, UmbracoMapper umbracoMapper)
        {
            _scopeProvider = scopeProvider;
            _umbracoMapper = umbracoMapper;
        }

        public LinkErrorDto Add(LinkErrorDto model)
        {
            var entity = _umbracoMapper.Map<LinkErrorDto, LinkErrorEntity>(model);
            using (var scope = _scopeProvider.CreateScope())
            {
                scope.Database.Insert(entity);
                scope.Complete();
            }

            return Get(entity.Id);
        }

        public LinkErrorDto Update(LinkErrorDto model)
        {
            var entity = _umbracoMapper.Map<LinkErrorDto, LinkErrorEntity>(model);
            using (var scope = _scopeProvider.CreateScope())
            {
                scope.Database.Update(entity);
                scope.Complete();
            }

            return Get(entity.Id);
        }

        public IEnumerable<LinkErrorDto> UpdateOrAdd(IEnumerable<LinkErrorDto> models)
        {
            //Make sure that we make a scope here so it's all bundled in one transaction
            using (var scope = _scopeProvider.CreateScope())
            {
                foreach (var model in models)
                {
                    if (model.Id == 0)
                        yield return Add(model);
                    else
                        yield return Update(model);
                }
            }
        }

        public void Delete(LinkErrorDto entity)
        {
            entity.IsDeleted = true;
            Update(entity);
        }

        public LinkErrorDto Get(int id)
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                return _umbracoMapper.Map<LinkErrorEntity, LinkErrorDto>(scope.Database.SingleById<LinkErrorEntity>(id));
            }
        }

        public LinkErrorDto GetByUrl(string url)
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                var sql = GetBaseQuery()
                    .Where<LinkErrorEntity>(error => error.Url.Equals(url));
                return _umbracoMapper.Map<LinkErrorEntity, LinkErrorDto>(scope.Database.SingleOrDefault<LinkErrorEntity>(sql));
            }
        }

        public IEnumerable<LinkErrorDto> GetAll(bool includeDeleted = false)
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                var sql = GetBaseQuery();
                if (!includeDeleted)
                {
                    sql = sql.Where<LinkErrorEntity>(it => !it.IsDeleted);
                }
                return _umbracoMapper.MapEnumerable<LinkErrorEntity, LinkErrorDto>(scope.Database.Fetch<LinkErrorEntity>(sql));
            }
        }

        private Sql<ISqlContext> GetBaseQuery()
        {
            return _scopeProvider.SqlContext.Sql()
                .SelectAll()
                .From<LinkErrorEntity>();
            return _scopeProvider.SqlContext.Sql()
                .Select("InboundLinkErrors.*, InboundLinkErrorReferrers.Referrer as 'LatestReferrer', InboundLinkErrorUserAgent.UserAgent as 'LatestUserAgent'")
                .From<LinkErrorEntity>()
                .LeftJoin<LinkErrorReferrerEntity>()
                .On<LinkErrorEntity, LinkErrorReferrerEntity>(left => left.Id, right => right.LinkErrorId)
                .LeftJoin<LinkErrorUserAgentEntity>()
                .On<LinkErrorEntity, LinkErrorUserAgentEntity>(left => left.Id, right => right.LinkErrorId)
                .Where("[InboundLinkErrorReferrers].LastAccessedTime is null or [InboundLinkErrorReferrers].LastAccessedTime = (select MAX(LastAccessedTime) from InboundLinkErrorReferrers where LinkErrorId = [InboundLinkErrors].Id group by LinkErrorId) and [InboundLinkErrorUserAgent].LastAccessedTime is null or [InboundLinkErrorUserAgent].LastAccessedTime = (select MAX(LastAccessedTime) from InboundLinkErrorUserAgent where LinkErrorId = [InboundLinkErrors].Id group by LinkErrorId)");
        }
    }
}
