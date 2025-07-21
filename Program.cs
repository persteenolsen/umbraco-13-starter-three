


using MembersUmbraco.Services;
using MembersUmbraco.Helpers;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// 10-06-2025: Register the Email send functionality by Services/EmailSender.cs and
// using the AppSettings.cs from Helpers and the section AppSettings from appsettings.json
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddScoped<IEmailSender, EmailSender>();



builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddDeliveryApi()
    .AddComposers()
    .Build();

WebApplication app = builder.Build();

// Note: Added to make the Umbraco Site more secure
app.Use(async (context, next) =>
{
    // Click-Jacking Protection
    // Checks if your site is allowed to be IFRAMEd by another site and 
    // then could be susceptible to click-jacking
    context.Response.Headers.Append("X-Frame-Options", "SAMEORIGIN");

    // Content/MIME Sniffing Protection
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");

    // Note: This header will make the Browser block the Bootstrap files 
    // and the layout of the Umbraco site will be lost
    // context.Response.Headers.Append("Content-Security-Policy", "img-src 'self' our.umbraco.com data: dashboard.umbraco.com; default-src 'self' our.umbraco.com marketplace.umbraco.com; script-src 'self'; style-src 'unsafe-inline' 'self'; font-src 'self'; connect-src 'self'; frame-src 'self'; ");
    
    await next();
});

// Note: Added to make the Umbraco Site more secure
// Protects for Cookie hijacking and protocol downgrade attacks
// Strict-Transport-Security Header (HSTS) is only used for Production when using HTTPS
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}


await app.BootUmbracoAsync();


app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseInstallerEndpoints();
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

await app.RunAsync();
