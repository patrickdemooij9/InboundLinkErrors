using System.Collections.Generic;
using InboundLinkErrors.Core.Models;
using InboundLinkErrors.Core.Models.Data;
using NPoco;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Umbraco.Core.Scoping;

namespace InboundLinkErrors.Core.Repositories
{
    public class LinkErrorsRepository
    {
        private readonly IScopeProvider _scopeProvider;

        public LinkErrorsRepository(IScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }

        public LinkErrorEntity Add(LinkErrorEntity entity)
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                scope.Database.Insert(entity);
                scope.Complete();
            }

            return entity;
        }

        public LinkErrorEntity Update(LinkErrorEntity entity)
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                scope.Database.Update(entity);
                scope.Complete();
            }

            return entity;
        }

        public void Delete(LinkErrorEntity entity)
        {
            entity.IsDeleted = true;
            Update(entity);
        }

        public LinkErrorEntity Get(int id)
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                return scope.Database.SingleById<LinkErrorEntity>(id);
            }
        }

        public LinkErrorEntity GetByUrl(string url)
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                var sql = GetBaseQuery()
                    .Where<LinkErrorEntity>(error => error.Url.Equals(url));
                return scope.Database.SingleOrDefault<LinkErrorEntity>(sql);
            }
        }

        public IEnumerable<LinkErrorEntity> GetAll(bool includeDeleted = false)
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                var sql = GetBaseQuery();
                if (!includeDeleted)
                {
                    sql = sql.Where<LinkErrorEntity>(it => !it.IsDeleted);
                }
                return scope.Database.Fetch<LinkErrorEntity>(sql);
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
