using System.Collections.Generic;
using InboundLinkErrors.Core.Models;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Umbraco.Core.Scoping;

namespace InboundLinkErrors.Core
{
    public class LinkErrorsRepository
    {
        private IScopeProvider _scopeProvider;
        private ILogger _logger;

        public LinkErrorsRepository(IScopeProvider scopeProvider, ILogger logger)
        {
            _scopeProvider = scopeProvider;
            _logger = logger;
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
                var sql = _scopeProvider.SqlContext.Sql()
                    .SelectAll()
                    .From<LinkErrorEntity>()
                    .Where<LinkErrorEntity>(error => error.Url.Equals(url));
                return scope.Database.SingleOrDefault<LinkErrorEntity>(sql);
            }
        }

        public IEnumerable<LinkErrorEntity> GetAll(bool includeDeleted = false)
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                var sql = _scopeProvider.SqlContext.Sql().SelectAll().From<LinkErrorEntity>();
                if (!includeDeleted)
                {
                    sql = sql.Where<LinkErrorEntity>(it => !it.IsDeleted);
                }
                return scope.Database.Fetch<LinkErrorEntity>(sql);
            }
        }
    }
}
