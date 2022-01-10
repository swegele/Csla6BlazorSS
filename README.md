# Csla6BlazorSS
Demo to show a few strange behaviors
Mostly you can just startup 

DEMO #1
- Set BlazorApp1 as startup project and debug.  Immediately you will notice the null reference error in the hosted DemoRefreshService,
	because the underlying HttpContext is null since no request is associated with the call to this service.
- Continue from there and the index page should load.  
- Now navigate to the Counter page and notice the call to the same business object works now (because there is an HttpContext now)
- Now simply remove the Blazor app's reference to Csla.AspNetCore.
- Debug, and the Hosted service call will now work without error

DEMO #2
- Set breakpoints in both AlphaInfoList and BetaInfoList [Fetch] methods at the "//**NOTE**" line so you can debug the GUID of the cxnManager
- See how the GUID changes which means it is not acting like a scoped service
- Now set a breakpoint and watch what happens in the DemoDataAdapterManager.cs in the GetExistingOrCreateNew method where it sets a value into ApplicationContext
	but then next chained call doesn't have the value in there.  This shows ApplicationContext is also Transient (created new each call even though all on server side)

DEMO #3
- Set LocalDataPortal to CreateScopePerCall = false by commenting current AddCsla section and uncommenting longer one that sets this option
- Debug app again - notice how GUID no longer changes in chained [Fetch] methods
- So now the server side scoped issue is gone
 