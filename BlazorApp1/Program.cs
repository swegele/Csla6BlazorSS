using Csla;
using Csla.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();

builder.Services.AddCsla(options =>
{
    options.AddServerSideBlazor();
});


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
