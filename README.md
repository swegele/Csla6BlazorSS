# Csla6BlazorSS
Demo to show possible issues for csla blazor server side


DEMO #1
- Pull latest from repo
- Run app
- Goto Counter page and notice unhandled error (hit F12 and see error)

- Now comment out the following line at the top of Counter.razor page "@inject BusinessLayer.BetaInfoListFactory _betaListFactory"
- Rerun app
- Everything works

??  what's going on
