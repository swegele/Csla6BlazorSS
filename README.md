# Csla6BlazorSS
Demo to show possible issues for csla blazor server side


DEMO #1
- Put a breakpoint in catch block of DemoRefreshService and start the app with no changes needed
- Notice the hosted service (which starts before a page loads) causes error in the Csla.Blazor.ApplicationContextManager.
- Now in Program.cs, uncomment the DEMO #1 custom application context manager 
- re-run and notice now the error moves down to the SetUser method.
