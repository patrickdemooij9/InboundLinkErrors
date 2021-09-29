using System;
using Umbraco.Cms.Core.Dashboards;

namespace InboundLinkErrors.Core
{
    public class LinkErrorsDashboard : IDashboard
    {
        public string Alias => "linkErrorsDashboard";
        public string View => "/App_Plugins/InboundLinkErrors/app.html";
        public string[] Sections => new string[] {"Content"};
        public IAccessRule[] AccessRules => Array.Empty<IAccessRule>();
    }
}
