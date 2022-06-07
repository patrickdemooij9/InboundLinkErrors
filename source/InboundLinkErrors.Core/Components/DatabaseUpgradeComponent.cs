using InboundLinkErrors.Core.Migrations;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;
using Umbraco.Cms.Infrastructure.Scoping;

namespace InboundLinkErrors.Core.Components
{
    public class DatabaseUpgradeComponent : IComponent
    {
        private readonly IMigrationPlanExecutor _migrationPlanExecutor;
        private readonly IScopeProvider _scopeProvider;
        private readonly IKeyValueService _keyValueService;
        private readonly IRuntimeState _runtimeState;

        public DatabaseUpgradeComponent(IMigrationPlanExecutor migrationPlanExecutor, IScopeProvider scopeProvider, IKeyValueService keyValueService, IRuntimeState runtimeState)
        {
            _migrationPlanExecutor = migrationPlanExecutor;
            _scopeProvider = scopeProvider;
            _keyValueService = keyValueService;
            _runtimeState = runtimeState;
        }

        public void Initialize()
        {
            if (_runtimeState.Level != RuntimeLevel.Run) return;

            var plan = new MigrationPlan("LinkErrorsMigration");
            plan.From(string.Empty)
                .To<InitialMigration>("state-initial")
                .To<ReferrerMigration>("state-referrer")
                .To<UserAgentMigration>("state-userAgent")
                .To<BatchingMigration>("state-batching");

            var upgrader = new Upgrader(plan);
            upgrader.Execute(_migrationPlanExecutor, _scopeProvider, _keyValueService);
        }

        public void Terminate()
        {

        }
    }
}
