using InboundLinkErrors.Core.Components;
using InboundLinkErrors.Core.ConfigurationProvider;
using InboundLinkErrors.Core.Interfaces;
using InboundLinkErrors.Core.Mappers;
using InboundLinkErrors.Core.Processor;
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
            composition.WithCollectionBuilder<MapDefinitionCollectionBuilder>()
                .Add<LinkErrorsMapDefinition>()
                .Add<LinkErrorsUserAgentMapDefinition>()
                .Add<LinkErrorsReferrerMapDefinition>()
                .Add<LinkErrorsViewMapDefinition>();

            composition.Dashboards().Add<LinkErrorsDashboard>();
            composition.Components().Append<DatabaseUpgradeComponent>();
            composition.Components().Append<LinkErrorsDatabaseSyncComponent>();
            composition.Components().Append<LinkErrorsCleanupComponent>();

            composition.Register<ILinkErrorsRepository, LinkErrorsRepository>(Lifetime.Request);
            composition.Register<ILinkErrorsProcessor, LinkErrorsProcessor>(Lifetime.Singleton);
            composition.Register<ILinkErrorsService, LinkErrorsService>(Lifetime.Request);
            composition.Register<LinkErrorsInjectedModule>(Lifetime.Request);
            composition.Register<IRedirectAdapter, UmbracoRedirectAdapter>(Lifetime.Request);
            composition.Register<ILinkErrorConfigurationProvider, LinkErrorConfigurationProvider>(Lifetime.Request);
        }
    }
}
