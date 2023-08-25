using Microsoft.AspNetCore.SignalR;

namespace TestSignalR.Hubs
{
    public abstract class HubBase : Hub
    {
        protected virtual Microsoft.AspNetCore.Http.HttpContext? GetCurrentHttpContext()
        {
            return Context.GetHttpContext();
        }
    }
}