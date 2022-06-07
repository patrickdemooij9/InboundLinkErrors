# InboundLinkErrors

Inbound link errors is a simple Umbraco Back-Office package that tracks 404 responses so that you can add a redirect to them. It includes a dashboard in Umbraco where the user is able to add it as a new redirect, delete it or hide it. 
Utilizes [ngTable](https://github.com/esvit/ng-table) for an AngularJs driven data table which includes ordering by column, simple pagination and searching.
Links are only tracked at the end of the pipeline and if the request resulted in a 404 response.

The dashboard for the inbound link errors has multiple filters that can be used:
- No filters: All non-media, non-hidden inbound link errors are shown.
- Media filter: Only media (all links having an extension) are shown.
- Hidden filter: Links that are hidden and non-hidden are shown.

InboundLinkErrors uses batching and only processes the 404 requests in bulk. This means that the system uses a lot less SQL queries and is a lot more efficient.

# Getting Started
Download the nuget package: ` Install-Package InboundLinkErrors ` or install it through Umbraco packages. After installing, make sure the following two lines can be found in your Web.config. Without it, the application will **not** be able to track your 404 responses.
```xml
<configuration>
   ...
  <system.webServer>
      ...
    <modules>
      ...
      <remove name="LinkErrorsHttpModule"/>
      <add name="LinkErrorsHttpModule" type="InboundLinkErrors.Core.LinkErrorsHttpModule, InboundLinkErrors" />
    </modules>
  </system.webServer>
</configuration>
```

# Configuration
There are various configuration that you can setup in your web config app settings. Simply add the following to your appsettings to change any of the configuration values:
```xml
<configuration>
   ...
   <appSettings>
      ...
      <add key="[Config key]" value="[Config value]" />
   </appSettings>
</configuration>
```

The configuration options are as follows:
- **InboundLinkErrors.TrackUserAgents (bool)**: Determines if user agents should be tracked. Even if this is turned off, the system will create tables for it. Default value: true
- **InboundLinkErrors.TrackReferrer (bool)**: Determines if referrers should be tracked. Even if this is turned off, the system will create tables for it. Default value: true
- **InboundLinkErrors.TrackMedia (bool)**: Determines if media is tracked. Disabling this will also get rid of the media filter on the overview. Default value: true
- **InboundLinkErrors.SyncStartupTime (int)**: Time in ms for sync task to start. The sync task is used to sync all 404 requests to the database. Default value: 60000 (1 min)
- **InboundLinkErrors.SyncInterval (int)**: Time in ms between syncs. Default value: 300000 (5 min)
- **InboundLinkErrors.CleanupStartTime (int)**: Time in ms for the cleanup task to start. The cleanup task is used to clean up any old 404 requests. Default value: 600000 (10 min)
- **InboundLinkErrors.CleanupInterval (int)**: Time in ms between the cleanups. Default value: 14400000 (4 hours)
- **InboundLinkErrors.CleanupAfterDays (int)**: Days after which old requests are cleaned up. Default value: 30

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
- Easily track what user-agents and/or referrers are causing your 404 pages

# Usage
InboundLinkErrors is located at the content section as one of the dashboards. When clicking on the dashboard called "Link errors", you'll be taken to the dashboard of InboundLinkErrors.

Once arrived, you'll usually see nothing in the table yet. This is probably because no requests with a 404 response have been made yet. When a 404 response is given, InboundLinkErrors will track it in the dashboard. You also have the option to filter on Hidden and Media requests.
![Dashboard](https://raw.githubusercontent.com/patrickdemooij9/InboundLinkErrors/master/package/InboundLinkErrors.PNG)

When a 404 response is tracked, you'll be able to perform 3 actions on it:
- Set Redirect: This allows you to redirect the given url to a different page on your website.
- Delete: This deletes the tracked 404 url. You can use this if you believe the page doesn't get used anymore.
- Hide: This will hide the tracked 404 url. You can use this when the url is most likely visited by a bot and is therefore not of use for you.

![Create redirect](https://raw.githubusercontent.com/patrickdemooij9/InboundLinkErrors/master/package/InboundLinkErrors2.PNG)

# Test website
You can run the website located in the project. You can use the following credentials to log in to the backoffice:
Email: admin@email.com
Password: Password123!

**Support:** [Issues/feature tracker](https://github.com/patrickdemooij9/InboundLinkErrors/issues)
