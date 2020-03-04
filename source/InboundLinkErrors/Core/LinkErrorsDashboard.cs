using System;
using Umbraco.Core.Dashboards;

namespace InboundLinkErrors.Core
{
    public class LinkErrorsDashboard : IDashboard
    {
        public string Alias => "linkErrorsDashboard";
        public string View => "/App_Plugins/LinkErrors/app.html";
        public string[] Sections => new string[] {"Content"};
        public IAccessRule[] AccessRules => Array.Empty<IAccessRule>();
    }
}
