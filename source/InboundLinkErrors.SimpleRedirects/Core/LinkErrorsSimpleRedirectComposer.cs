using InboundLinkErrors.Core;
using InboundLinkErrors.Core.Interfaces;
using System;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace InboundLinkErrors.SimpleRedirects.Core
{
    [ComposeAfter(typeof(LinkErrorsUserComposer))]
    public class LinkErrorsSimpleRedirectComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register<IRedirectAdapter, LinkErrorsSimpleRedirectAdapter>(Lifetime.Request);
        }
    }
}
