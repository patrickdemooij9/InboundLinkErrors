# InboundLinkErrors

Inbound link errors is a simple Umbraco Back-Office package that tracks 404 responses so that you can add a redirect to them. It includes a dashboard in Umbraco where the user is able to add it as a new redirect, delete it or hide it. 
Utilizes [ngTable][ngTableLink] for an AngularJs driven data table which includes ordering by column, simple pagination and searching.
Links are only tracked at the end of the pipeline and if the request resulted in a 404 response.

The dashboard for the inbound link errors has multiple filters that can be used:
- No filters: All non-media, non-hidden inbound link errors are shown.
- Media filter: Only media (all links having an extension) are shown.
- Hidden filter: Links that are hidden and non-hidden are shown.

# Getting Started
Just download the nuget package: ` Install-Package InboundLinkErrors ` and enjoy your amazing dashboard in Umbraco.

If you want to connect the InboundLinkErrors to the SimpleRedirects plugin, you can download the following nuget package:  ` Install InboundLinkErrors.SimpleRedirects `. This will connect the InboundLinkErrors package to the SimpleRedirects package.

