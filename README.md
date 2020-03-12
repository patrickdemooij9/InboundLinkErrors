# InboundLinkErrors

[![Build status](https://ci.appveyor.com/api/projects/status/u3267xahfofk77y5?svg=true)](https://ci.appveyor.com/project/patrickdemooij9/inboundlinkerrors)

Inbound link errors is a simple Umbraco Back-Office package that tracks 404 responses so that you can add a redirect to them. It includes a dashboard in Umbraco where the user is able to add it as a new redirect, delete it or hide it. 
Utilizes [ngTable](https://github.com/esvit/ng-table) for an AngularJs driven data table which includes ordering by column, simple pagination and searching.
Links are only tracked at the end of the pipeline and if the request resulted in a 404 response.

The dashboard for the inbound link errors has multiple filters that can be used:
- No filters: All non-media, non-hidden inbound link errors are shown.
- Media filter: Only media (all links having an extension) are shown.
- Hidden filter: Links that are hidden and non-hidden are shown.

# Getting Started
Just download the nuget package: ` Install-Package InboundLinkErrors ` and enjoy your amazing dashboard in Umbraco.

# Configuration
By default, the plugin will write your new redirects to the Url tracker of Umbraco. You are however able to change this to with any of the following packages:

### [SimpleRedirects](https://github.com/patrickdemooij9/SimpleRedirects)
If you want to connect the InboundLinkErrors to the SimpleRedirects plugin, you can download the following nuget package:  ` Install InboundLinkErrors.SimpleRedirects `. This will connect the InboundLinkErrors package to the SimpleRedirects package.

### [Skybrud Umbraco Redirects](https://github.com/skybrud/Skybrud.Umbraco.Redirects)
Currently work in progress!

# Features
- Simple dashboard which lets you manage your inbound link errors
- Ordering on the amount of hits, url and last hit time
- Searching on different urls
- Easy way to add redirects on the hit 404 pages
- Hide unwanted 404 pages

# Usage
InboundLinkErrors is located at the content section as one of the dashboards. When clicking on the dashboard called "Link errors", you'll be taken to the dashboard of InboundLinkErrors.

Once arrived, you'll usually see nothing in the table yet. This is probably because no requests with a 404 response have been made yet. When a 404 response is given, InboundLinkErrors will track it in the dashboard. You also have the option to filter on Hidden and Media requests.

When a 404 response is tracked, you'll be able to perform 3 actions on it:
- Set Redirect: This allows you to redirect the given url to a different page on your website.
- Delete: This deletes the tracked 404 url. You can use this if you believe the page doesn't get used anymore.
- Hide: This will hide the tracked 404 url. You can use this when the url is most likely visited by a bot and is therefore not of use for you.

**Support:** [Issues/feature tracker](https://github.com/patrickdemooij9/InboundLinkErrors/issues)
