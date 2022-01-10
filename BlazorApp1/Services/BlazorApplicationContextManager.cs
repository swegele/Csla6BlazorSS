using Csla;
using Csla.Core;

namespace BlazorApp1.Services;

public class BlazorApplicationContextManager : IContextManager
{
    private System.Security.Principal.IPrincipal? Principal { get; set; }
    private ContextDictionary LocalContext { get; set; } = new ContextDictionary();
    private ContextDictionary ClientContext { get; set; } = new ContextDictionary();
    private ApplicationContext? _applicationContext { get; set; }
    private Guid _guid = Guid.NewGuid(); //testing to see unique instances in DI

    /// <summary>
    /// Creates an instance of the object, initializing it
    /// with the required IServiceProvider.
    /// </summary>
    /// <param name="httpContextAccessor">HttpContext accessor</param>
    public BlazorApplicationContextManager()
    {
    }

    /// <summary>
    /// Gets the current principal.
    /// </summary>
    public System.Security.Principal.IPrincipal GetUser()
    {
        var result = Principal;
        if (result == null)
        {
            result = new Csla.Security.CslaClaimsPrincipal();
            SetUser(result);
        }
        return result;
    }

    /// <summary>
    /// Sets the current principal.
    /// </summary>
    /// <param name="principal">Principal object.</param>
    public void SetUser(System.Security.Principal.IPrincipal principal)
    {
        Principal = principal;
    }

    /// <summary>
    /// Gets the local context.
    /// </summary>
    public ContextDictionary GetLocalContext()
    {
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
    /// Gets or sets a reference to the current ApplicationContext.
    /// </summary>
    public virtual ApplicationContext ApplicationContext
    {
        get
        {
            return _applicationContext;
        }
        set
        {
            _applicationContext = value;
        }
    }

    public bool IsValid => true;
}
