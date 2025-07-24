# umbraco-13-starter-three

Last Updated

24-07-2025

A Website by Umbraco CMS 13 with Member Registration, Login and Forgot Password serving as a Starter

# Create a global json

dotnet new globaljson --sdk-version 8.0.203 --force

# Installation

- dotnet new install Umbraco.Templates::13.*

- dotnet new umbraco --name MembersUmbraco

- cd MembersUmbraco

- dotnet run

- Open https : // localhost:44319 in your Browser and follow the instructions

# Functionality of the Website

- Simple Website with an Umbraco Backend
- Member Registration
- Member Confirm Registration
- Member Login
- Forgot Password
- Edit Members own Profile when logged in

# Tech used for creating the Website

- .NET 8
- Umbraco CMS 13
- SurfaceControllers
- RenderController
- Input Validation by Model Anotation
- Input Validation by Frontend JS
- SQLite DB for both Dev + Prod
- A traditional Webhotel for hosting
- VS Code

# Development

dotnet run

# Production

Create a self contained build for production at the remote server / traditionel web hotel

dotnet publish membersumbraco.csproj --configuration Release --runtime win-x86 --self-contained

Upload the build to remote server

At my remote servers the web.config needs to be without the folowing:

hostingModel="inprocess"

Upload the SQLite DB to umbraco/Data at the remote server

# Settings for Production

Login to your Umbraco and perform a Health Check under Settings

Open your web.config file at your Web hotel and be aware of the Excessive Headers that:

- Could be revealing information in its headers that gives away unnecessary details about the technology used to build and host

- Add code to web.config which contains:

- httpProtocol - customHeaders - remove - name=X-Powered-By

- security - requestFiltering - RemoveServerHeader=true

Take a look in file Program.cs and the code about security by the HTTP Headers:

- Click-Jacking Protection

- Content / MIME Sniffing Protection

- Cookie hijacking and protocol downgrade attacks Protection

Go to appsettings.json and make sure that your Umbraco Site have the settings:

- Hosting:Debug:false

- UseHTTPS:true

- WebRouting:UmbracoApplicationUrl:your.domain.com

- Go to MembersUmbraco . csproj and enable the ItemGroup / compile:

Note: Also check your settings in appsettings.Development.json for

- Hosting:Debug

# Performance => Slow first Page load

- Used Google Page Speed

- Eliminated Render-Blocking

- I optimized the loading CSS and JS in the Master View

- In web.config I added Rewrite Rule to force HTTP request to HTTPS

- In web.config I added applicationInitialization to prevent cold start ( IIS seems to igore it! )

- As a workaround I set up a crone job to make a request every few minutes to prevent cold start

# Custom Error Pages

- 404 Error Page by web.config and Umbraco

- 500 Error Page by web.config and 500.html

- 503 Error Page by web.config and 503.html

# Sync your remote Prod changes to your local Dev

Yor will need to commit the changes from the sqlite-wal file to sqlite.db at the Web Hotel before downloading to local Dev:

- Open web.config and save - This is a work around for stop / start the app pool

- Download the sqlite.db

- Download Views, media files or whatever files that you worked with at Prod

# Models Builder - Configuration - Tips and Tricks

Be sure that the Models for the Document Types are available and Recognized in VS Code:

- Take a look at the settings for ModelsBuilder in appsetting.Development

- Make sure to create the folder: umbraco / models to match the "ModelsDirectory": "~/umbraco/models"

- Make sure you have the setting: "ModelsMode": "SourceCodeManual" for generate the .cs files

- Make a dotnet build and then dotnet run and open the Admin Section of your site which will work even the site will not work before the models will be generated !!!

- Go to the Models Builder in the Admin Section of your site and click: Regenerate Models

- Take a look in the folder: umbraco / models where the .cs files / models were created by VS Code

- Stop your site and make a dotnet run to see your site is running

- Note: The cs files / models must be ignored before Release Build / Production. This must be doing manually in the ItemGroup / compile in the MembersUmbraco . csproj !!!

Happy use of Umbraco :-)

