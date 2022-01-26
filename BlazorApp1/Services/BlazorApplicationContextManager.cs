﻿using Csla;
using Csla.Core;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Security.Principal;

namespace BlazorApp1.Services;

/// <summary>
/// Application context manager that uses HttpContextAccessor when 
/// resolving HttpContext to store context values.
/// </summary>
public class BlazorApplicationContextManager : IContextManager, IDisposable
{
    private ContextDictionary LocalContext { get; set; }
    private ContextDictionary ClientContext { get; set; }
    private IPrincipal CurrentPrincipal { get; set; }
    private readonly ClaimsPrincipal UnauthenticatedPrincipal = new();
    private bool disposedValue;

    /// <summary>
    /// Gets the current HttpContext instance.
    /// </summary>
    protected AuthenticationStateProvider AuthenticationStateProvider { get; private set; }

    /// <summary>
    /// Gets or sets a reference to the current ApplicationContext.
    /// </summary>
    public ApplicationContext ApplicationContext { get; set; }

    /// <summary>
    /// Creates an instance of the object, initializing it
    /// with the required IServiceProvider.
    /// </summary>
    /// <param name="authenticationStateProvider">AuthenticationStateProvider service</param>
    public BlazorApplicationContextManager(AuthenticationStateProvider authenticationStateProvider)
    {
        AuthenticationStateProvider = authenticationStateProvider;
        CurrentPrincipal = UnauthenticatedPrincipal;
        AuthenticationStateProvider.AuthenticationStateChanged += AuthenticationStateProvider_AuthenticationStateChanged;
        InitializeUser();
    }

    private void InitializeUser()
    {
        if (AuthenticationStateProvider is IHostEnvironmentAuthenticationStateProvider hostEnvironmentAuthProvider)
        {
            //i've no idea what i'm doing except to call set before get like error says
            var task = new Task<AuthenticationState>(() => new AuthenticationState(UnauthenticatedPrincipal));

            //the following set triggers the authentication state changed so why do it again below?
            hostEnvironmentAuthProvider.SetAuthenticationState(task);
        }
        else
        {
            //??
            AuthenticationStateProvider_AuthenticationStateChanged(AuthenticationStateProvider.GetAuthenticationStateAsync());
        }
    }

    private void AuthenticationStateProvider_AuthenticationStateChanged(Task<AuthenticationState> task)
    {
        task.ContinueWith((t) =>
        {
            if (task.IsCompletedSuccessfully && task.Result != null)
                CurrentPrincipal = task.Result.User;
            else
                CurrentPrincipal = UnauthenticatedPrincipal;
        });
    }

    /// <summary>
    /// Gets a value indicating whether this
    /// context manager is valid for use in
    /// the current environment.
    /// </summary>
    public bool IsValid
    {
        get { return true; }
    }

    /// <summary>
    /// Gets the current principal.
    /// </summary>
    public IPrincipal GetUser()
    {
        return CurrentPrincipal;
    }

    /// <summary>
    /// Not supported. Use the aspnetcore framework to set 
    /// the current principal.
    /// </summary>
    /// <param name="principal">Principal object.</param>
    public virtual void SetUser(IPrincipal principal)
    {        
        throw new NotSupportedException(nameof(SetUser));
    }

    /// <summary>
    /// Gets the local context.
    /// </summary>
    public ContextDictionary GetLocalContext()
    {
        if (LocalContext == null)
            LocalContext = new ContextDictionary();
        return LocalContext;
    }

    /// <summary>
    /// Sets the local context.
    /// </summary>
    /// <param name="localContext">Local context.</param>
    public void SetLocalContext(ContextDictionary localContext)
    {
        LocalContext = localContext;
    }

    /// <summary>
    /// Gets the client context.
    /// </summary>
    /// <param name="executionLocation"></param>
    public ContextDictionary GetClientContext(ApplicationContext.ExecutionLocations executionLocation)
    {
        if (ClientContext == null)
            ClientContext = new ContextDictionary();
        return ClientContext;
    }

    /// <summary>
    /// Sets the client context.
    /// </summary>
    /// <param name="clientContext">Client context.</param>
    /// <param name="executionLocation"></param>
    public void SetClientContext(ContextDictionary clientContext, ApplicationContext.ExecutionLocations executionLocation)
    {
        ClientContext = clientContext;
    }

    /// <summary>
    /// Dispose this object's resources.
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                AuthenticationStateProvider.AuthenticationStateChanged -= AuthenticationStateProvider_AuthenticationStateChanged;
            }
            disposedValue = true;
        }
    }

    /// <summary>
    /// Dispose this object's resources.
    /// </summary>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
