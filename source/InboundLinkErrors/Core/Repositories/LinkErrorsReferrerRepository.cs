using InboundLinkErrors.Core.Models;
using System.Collections.Generic;
using InboundLinkErrors.Core.Models.Data;
using Umbraco.Core.Persistence;
using Umbraco.Core.Scoping;

namespace InboundLinkErrors.Core.Repositories
{
    public class LinkErrorsReferrerRepository
    {
        private readonly IScopeProvider _scopeProvider;

        public LinkErrorsReferrerRepository(IScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }

        public LinkErrorReferrerEntity Add(LinkErrorReferrerEntity entity)
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                scope.Database.Insert(entity);
                scope.Complete();
            }

            return entity;
        }

        public LinkErrorReferrerEntity Update(LinkErrorReferrerEntity entity)
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                scope.Database.Update(entity);
                scope.Complete();
            }

            return entity;
        }

        public LinkErrorReferrerEntity Get(int id)
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                return scope.Database.SingleById<LinkErrorReferrerEntity>(id);
            }
        }

        public LinkErrorReferrerEntity Get(int linkErrorId, string referrer)
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                var sql = scope.SqlContext.Sql().SelectAll().From<LinkErrorReferrerEntity>()
                    .Where<LinkErrorReferrerEntity>(it =>
                        it.LinkErrorId == linkErrorId && it.Referrer.Equals(referrer));
                return scope.Database.FirstOrDefault<LinkErrorReferrerEntity>(sql);
            }
        }

        public IEnumerable<LinkErrorReferrerEntity> GetAll()
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                return scope.Database.Fetch<LinkErrorReferrerEntity>();
            }
        }

        public IEnumerable<LinkErrorReferrerEntity> GetAllByLinkError(int linkErrorId)
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                var sql = scope.SqlContext.Sql().SelectAll().From<LinkErrorReferrerEntity>().Where<LinkErrorReferrerEntity>(it => it.LinkErrorId == linkErrorId);
                return scope.Database.Fetch<LinkErrorReferrerEntity>(sql);
            }
        }
    }
}
