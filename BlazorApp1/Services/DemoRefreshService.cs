using BusinessLayer;
using Microsoft.AspNetCore.Components.Authorization;
using System.Diagnostics;

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
        int time = 2; //2 minutes for demo purposes

        _timer = new Timer(
            DoWork,
            null,
            TimeSpan.Zero,
            TimeSpan.FromMinutes(time)
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
                var authStateProvider = sp.GetRequiredService<AuthenticationStateProvider>();

                //set user to blank claims principal
                var task = new Task<AuthenticationState>(() => new AuthenticationState(new System.Security.Claims.ClaimsPrincipal()));
                (authStateProvider as IHostEnvironmentAuthenticationStateProvider).SetAuthenticationState(task);

                var betaFactory = sp.GetRequiredService<BetaInfoListFactory>();

                //do call to BL
                LoadFreshList(betaFactory).Wait();
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

    private async Task LoadFreshList(BetaInfoListFactory factory)
    {
        //do some business stuff
        var translations = await Task.FromResult(factory.GetAll());
    }

}
