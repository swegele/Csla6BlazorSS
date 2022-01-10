using BlazorApp1.Data;
using BlazorApp1.Services;
using Csla;
using Csla.Configuration;
using Csla.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();


//COMMENT this for DEMO #3 and then uncomment below one
builder.Services.AddCsla(cslaOptions =>
    cslaOptions
        .AddServerSideBlazor());
//builder.Services.AddCsla(cslaOptions =>
//    cslaOptions
//        .AddServerSideBlazor()
//        .DataPortal()
//            .AddServerSideDataPortal()
//            .UseLocalProxy(proxyOptions =>
//                proxyOptions.CreateScopePerCall = false));


//four business object factories and dataadaptermanager
builder.Services.AddScoped<BusinessLayer.AlphaInfoFactory>();
builder.Services.AddScoped<BusinessLayer.AlphaInfoListFactory>();
builder.Services.AddScoped<BusinessLayer.BetaInfoFactory>();
builder.Services.AddScoped<BusinessLayer.BetaInfoListFactory>();
builder.Services.AddScoped<BusinessLayer.DemoDataAdapterManagerFactory>();

//matching dataportal services
builder.Services.AddScoped<DataPortal<BusinessLayer.AlphaInfo>>();
builder.Services.AddScoped<DataPortal<BusinessLayer.AlphaInfoList>>();
builder.Services.AddScoped<DataPortal<BusinessLayer.BetaInfo>>();
builder.Services.AddScoped<DataPortal<BusinessLayer.BetaInfoList>>();

//DEMO 4 - UNCOMMENT
//builder.Services.AddScoped<Csla.Core.IContextManager, BlazorApplicationContextManager>();

builder.Services.AddSingleton<WeatherForecastService>();


builder.Services.AddHostedService<DemoRefreshService>(); //refresh service which makes a call without an httpcontext

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
