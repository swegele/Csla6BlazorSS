using BlazorApp1.Services;
using BusinessLayer.ExtensionMethods;
using Csla.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();

builder.Services.AddCsla(options =>
{
    options.AddServerSideBlazor();
    options.AddAspNetCore();
});

//auto register business object factories -  this extension method should be renamed when it goes live to match the assembly name
builder.Services.AutoRegisterBusinessObjectFactories();

builder.Services.AddScoped<BusinessLayer.DemoDataAdapterManagerFactory>();

builder.Services.AddHostedService<DemoRefreshService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
