using BusinessLayer;
using Csla;
using Microsoft.AspNetCore.Components.Authorization;
using System.Diagnostics;
using System.Security.Claims;

namespace BlazorApp1.Services;

public class DemoRefreshService : IHostedService
{
    private System.Threading.Timer? _timer;
    private readonly IServiceScopeFactory _serviceProvider;

    public DemoRefreshService(IServiceScopeFactory serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // timer repeates call to DoWork on scheduled basis
        int refreshMinutes = 20;

        _timer = new Timer(
            DoWork,
            null,
            TimeSpan.Zero,
            TimeSpan.FromMinutes(refreshMinutes)
        );

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    bool _doingWork = false;
    private void DoWork(object? state)
    {
        //do a brief check to not start multiple at same time (shouldn't happen in normal course of business because minimum time...but during dev debugging it stucks)
        if (_doingWork)
            return;
        else
            _doingWork = true;

        try
        {
            //our api calls are in scoped services while this ihostedservice is singleton...so we have to create a scope here so we can fetch scoped services
            using (var scope = _serviceProvider.CreateScope())
            {
                var sp = scope.ServiceProvider;


                ////FOR TESTING LOGIN DURING HOSTED SERVICE CALLS
                //var authStateProvider = sp.GetRequiredService<AuthenticationStateProvider>();
                ////set user to blank claims principal
                //var claims = new List<Claim>
                //{
                //        new Claim(ClaimTypes.Name, "Sean")
                //};
                //var claimsIdentity = new ClaimsIdentity(claims, "Custom");
                //var principal = new ClaimsPrincipal();
                //principal.AddIdentities(new List<ClaimsIdentity>() { claimsIdentity });
                //var task = Task.FromResult(new AuthenticationState(principal));
                //(authStateProvider as IHostEnvironmentAuthenticationStateProvider).SetAuthenticationState(task);

                var appContext = sp.GetRequiredService<ApplicationContext>();

                //do call to BL
                LoadFreshList(appContext).Wait();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Oh Snap error in hosted service DoWork method - " + ex.GetBaseException().Message);
        }
        finally
        {
            _doingWork = false;
        }
    }

    private async Task LoadFreshList(ApplicationContext appContext)
    {
        //do some business stuff
        var translations = await Task.FromResult(BetaInfoList.GetAll(appContext));
    }

}
