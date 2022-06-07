using InboundLinkErrors.Core.Components;
using InboundLinkErrors.Core.ConfigurationProvider;
using InboundLinkErrors.Core.Interfaces;
using InboundLinkErrors.Core.Mappers;
using InboundLinkErrors.Core.Middleware;
using InboundLinkErrors.Core.Options;
using InboundLinkErrors.Core.Processor;
using InboundLinkErrors.Core.Repositories;
using InboundLinkErrors.Core.Services;
using InboundLinkErrors.Core.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Web.Common.ApplicationBuilder;
using Umbraco.Extensions;

namespace InboundLinkErrors.Core
{
    public class LinkErrorsUserComposer : IComposer
    {
        public void Compose(IUmbracoBuilder composition)
        {
            composition.WithCollectionBuilder<MapDefinitionCollectionBuilder>()
                .Add<LinkErrorsMapDefinition>()
                .Add<LinkErrorsUserAgentMapDefinition>()
                .Add<LinkErrorsReferrerMapDefinition>()
                .Add<LinkErrorsViewMapDefinition>();

            composition.Dashboards().Add<LinkErrorsDashboard>();
            composition.Components().Append<DatabaseUpgradeComponent>();

            composition.Services.AddUnique<ILinkErrorsRepository, LinkErrorsRepository>();
            composition.Services.AddUnique<ILinkErrorsProcessor, LinkErrorsProcessor>();
            composition.Services.AddUnique<ILinkErrorsService, LinkErrorsService>();
            composition.Services.AddUnique<IRedirectAdapter, UmbracoRedirectAdapter>();
            composition.Services.AddUnique<ILinkErrorConfigurationProvider, LinkErrorConfigurationProvider>();

            composition.Services.AddHostedService<LinkErrorsCleanupTask>();
            composition.Services.AddHostedService<LinkErrorsDatabaseSyncTask>();

            composition.Services.Configure<LinkErrorsOptions>(composition.Config.GetSection(
                LinkErrorsOptions.Position));

            composition.Services.Configure<UmbracoPipelineOptions>(options => {
                options.AddFilter(new UmbracoPipelineFilter(
                    "InboundLinkErrors",
                    applicationBuilder => { },
                    applicationBuilder => {
                        applicationBuilder.UseMiddleware<LinkErrorsMiddleware>();
                    },
                    applicationBuilder => { }
                ));
            });
        }
    }
}
