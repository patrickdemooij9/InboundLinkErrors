using System.Collections.Generic;
using System.Linq;
using InboundLinkErrors.Core.Interfaces;
using InboundLinkErrors.Core.Models.Data;
using InboundLinkErrors.Core.Models.Dto;
using NPoco;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Extensions;

namespace InboundLinkErrors.Core.Repositories
{
    public class LinkErrorsRepository : ILinkErrorsRepository
    {
        private readonly IScopeProvider _scopeProvider;
        private readonly IUmbracoMapper _umbracoMapper;

        public LinkErrorsRepository(IScopeProvider scopeProvider, IUmbracoMapper umbracoMapper)
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

                foreach (var view in model.Views)
                {
                    scope.Database.Insert(_umbracoMapper.Map<LinkErrorViewDto, LinkErrorViewEntity>(view, (context) => context.Items.Add("LinkErrorId", entity.Id)));
                }
                foreach (var userAgent in model.UserAgents)
                {
                    scope.Database.Insert(_umbracoMapper.Map<LinkErrorUserAgentDto, LinkErrorUserAgentEntity>(userAgent, (context) => context.Items.Add("LinkErrorId", entity.Id)));
                }
                foreach (var referrer in model.Referrers)
                {
                    scope.Database.Insert(_umbracoMapper.Map<LinkErrorReferrerDto, LinkErrorReferrerEntity>(referrer, (context) => context.Items.Add("LinkErrorId", entity.Id)));
                }

                scope.Complete();
            }

            return Get(entity.Id);
        }

        public LinkErrorDto Update(LinkErrorDto model)
        {
            var entity = _umbracoMapper.Map<LinkErrorDto, LinkErrorEntity>(model);
            var currentModel = Get(entity.Id);

            using (var scope = _scopeProvider.CreateScope())
            {
                scope.Database.Update(entity);

                foreach (var view in model.Views.Select(it => _umbracoMapper.Map<LinkErrorViewDto, LinkErrorViewEntity>(it, (context) => context.Items.Add("LinkErrorId", entity.Id))))
                {
                    if (scope.Database.Exists<LinkErrorViewEntity>(view))
                        scope.Database.Update(view);
                    else
                        scope.Database.Insert(view);
                }

                foreach (var view in currentModel.Views.Where(it => model.Views.All(v => it.Date.Date != v.Date.Date)))
                {
                    scope.Database.Delete(_umbracoMapper.Map<LinkErrorViewDto, LinkErrorViewEntity>(view, (context) => context.Items.Add("LinkErrorId", entity.Id)));
                }
                foreach (var userAgent in model.UserAgents.Select(it => _umbracoMapper.Map<LinkErrorUserAgentDto, LinkErrorUserAgentEntity>(it, (context) => context.Items.Add("LinkErrorId", entity.Id))))
                {
                    if (userAgent.Id != 0)
                        scope.Database.Update(userAgent);
                    else
                        scope.Database.Insert(userAgent);
                }
                foreach (var userAgent in currentModel.UserAgents.Where(it => model.UserAgents.All(v => it.Id != v.Id)))
                {
                    scope.Database.Delete(_umbracoMapper.Map<LinkErrorUserAgentDto, LinkErrorUserAgentEntity>(userAgent, (context) => context.Items.Add("LinkErrorId", entity.Id)));
                }

                foreach (var referrer in model.Referrers.Select(it => _umbracoMapper.Map<LinkErrorReferrerDto, LinkErrorReferrerEntity>(it, (context) => context.Items.Add("LinkErrorId", entity.Id))))
                {
                    if (referrer.Id != 0)
                        scope.Database.Update(referrer);
                    else
                        scope.Database.Insert(referrer);
                }
                foreach (var referrer in currentModel.Referrers.Where(it => model.Referrers.All(v => it.Id != v.Id)))
                {
                    scope.Database.Delete(_umbracoMapper.Map<LinkErrorReferrerDto, LinkErrorReferrerEntity>(referrer, (context) => context.Items.Add("LinkErrorId", entity.Id)));
                }

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

        public void Delete(LinkErrorDto model)
        {
            var entity = _umbracoMapper.Map<LinkErrorDto, LinkErrorEntity>(model);
            using (var scope = _scopeProvider.CreateScope())
            {
                foreach (var view in model.Views.Select(it => _umbracoMapper.Map<LinkErrorViewDto, LinkErrorViewEntity>(it, (context) => context.Items.Add("LinkErrorId", entity.Id))))
                {
                    scope.Database.Delete(view);
                }
                foreach (var userAgent in model.UserAgents.Select(it => _umbracoMapper.Map<LinkErrorUserAgentDto, LinkErrorUserAgentEntity>(it, (context) => context.Items.Add("LinkErrorId", entity.Id))))
                {
                    scope.Database.Delete(userAgent);
                }
                foreach (var referrer in model.Referrers.Select(it => _umbracoMapper.Map<LinkErrorReferrerDto, LinkErrorReferrerEntity>(it, (context) => context.Items.Add("LinkErrorId", entity.Id))))
                {
                    scope.Database.Delete(referrer);
                }

                scope.Database.Delete(entity);

                scope.Complete();
            }
        }

        public LinkErrorDto Get(int id)
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                return Map(scope.Database.SingleById<LinkErrorEntity>(id), scope);
            }
        }

        public IEnumerable<LinkErrorDto> GetByUrl(params string[] urls)
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                var sql = GetBaseQuery()
                    .Where<LinkErrorEntity>(error => urls.Contains(error.Url));
                return scope.Database.Fetch<LinkErrorEntity>(sql).Select(it => Map(it, scope)).ToArray();
            }
        }

        public IEnumerable<LinkErrorDto> GetAll()
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                var sql = GetBaseQuery();
                return scope.Database.Fetch<LinkErrorEntity>(sql).Select(it => Map(it, scope)).ToArray();
            }
        }

        private IEnumerable<LinkErrorViewEntity> GetViews(int linkErrorId, IScope scope)
        {
            return scope.Database.Fetch<LinkErrorViewEntity>(scope.SqlContext.Sql().SelectAll().From<LinkErrorViewEntity>().Where<LinkErrorViewEntity>(it => it.LinkErrorId == linkErrorId));
        }

        private IEnumerable<LinkErrorUserAgentEntity> GetUserAgents(int linkErrorId, IScope scope)
        {
            return scope.Database.Fetch<LinkErrorUserAgentEntity>(scope.SqlContext.Sql().SelectAll().From<LinkErrorUserAgentEntity>().Where<LinkErrorUserAgentEntity>(it => it.LinkErrorId == linkErrorId));
        }

        private IEnumerable<LinkErrorReferrerEntity> GetReferrers(int linkErrorId, IScope scope)
        {
            return scope.Database.Fetch<LinkErrorReferrerEntity>(scope.SqlContext.Sql().SelectAll().From<LinkErrorReferrerEntity>().Where<LinkErrorReferrerEntity>(it => it.LinkErrorId == linkErrorId));
        }

        private LinkErrorDto Map(LinkErrorEntity entity, IScope scope)
        {
            var model = _umbracoMapper.Map<LinkErrorEntity, LinkErrorDto>(entity);
            model.Views = _umbracoMapper.MapEnumerable<LinkErrorViewEntity, LinkErrorViewDto>(GetViews(model.Id, scope));
            model.UserAgents = _umbracoMapper.MapEnumerable<LinkErrorUserAgentEntity, LinkErrorUserAgentDto>(GetUserAgents(model.Id, scope));
            model.Referrers = _umbracoMapper.MapEnumerable<LinkErrorReferrerEntity, LinkErrorReferrerDto>(GetReferrers(model.Id, scope));
            return model;
        }

        private Sql<ISqlContext> GetBaseQuery()
        {
            return _scopeProvider.SqlContext.Sql()
                .SelectAll()
                .From<LinkErrorEntity>();
        }
    }
}
