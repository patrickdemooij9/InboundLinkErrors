using System.Collections.Generic;
using InboundLinkErrors.Core.Models;
using InboundLinkErrors.Core.Models.Data;
using Umbraco.Core.Persistence;
using Umbraco.Core.Scoping;

namespace InboundLinkErrors.Core.Repositories
{
    public class LinkErrorsUserAgentRepository
    {
        private readonly IScopeProvider _scopeProvider;

        public LinkErrorsUserAgentRepository(IScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }

        public LinkErrorUserAgentEntity Add(LinkErrorUserAgentEntity entity)
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                scope.Database.Insert(entity);
                scope.Complete();
            }

            return entity;
        }

        public LinkErrorUserAgentEntity Update(LinkErrorUserAgentEntity entity)
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                scope.Database.Update(entity);
                scope.Complete();
            }

            return entity;
        }

        public LinkErrorUserAgentEntity Get(int id)
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                return scope.Database.SingleById<LinkErrorUserAgentEntity>(id);
            }
        }

        public LinkErrorUserAgentEntity Get(int linkErrorId, string userAgent)
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                var sql = scope.SqlContext.Sql().SelectAll().From<LinkErrorUserAgentEntity>()
                    .Where<LinkErrorUserAgentEntity>(it =>
                        it.LinkErrorId == linkErrorId && it.UserAgent.Equals(userAgent));
                return scope.Database.FirstOrDefault<LinkErrorUserAgentEntity>(sql);
            }
        }

        public IEnumerable<LinkErrorUserAgentEntity> GetAll()
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                return scope.Database.Fetch<LinkErrorUserAgentEntity>();
            }
        }

        public IEnumerable<LinkErrorUserAgentEntity> GetAllByLinkError(int linkErrorId)
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                var sql = scope.SqlContext.Sql().SelectAll().From<LinkErrorUserAgentEntity>().Where<LinkErrorUserAgentEntity>(it => it.LinkErrorId == linkErrorId);
                return scope.Database.Fetch<LinkErrorUserAgentEntity>(sql);
            }
        }
    }
}
