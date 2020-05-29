using InboundLinkErrors.Core.Components;
using InboundLinkErrors.Core.Interfaces;
using InboundLinkErrors.Core.Repositories;
using InboundLinkErrors.Core.Services;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Mapping;
using Umbraco.Web;

namespace InboundLinkErrors.Core
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Boot)]
    public class LinkErrorsUserComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.WithCollectionBuilder<MapDefinitionCollectionBuilder>().Add<LinkErrorsMapDefinition>();

            composition.Dashboards().Add<LinkErrorsDashboard>();
            composition.Components().Append<DatabaseUpgradeComponent>();

            composition.Register<LinkErrorsRepository>(Lifetime.Request);
            composition.Register<LinkErrorsService>(Lifetime.Request);
            composition.Register<LinkErrorsReferrerRepository>(Lifetime.Request);
            composition.Register<LinkErrorsReferrerService>(Lifetime.Request);
            composition.Register<LinkErrorsInjectedModule>(Lifetime.Request);
            composition.Register<IRedirectAdapter, UmbracoRedirectAdapter>(Lifetime.Request);
        }
    }
}
