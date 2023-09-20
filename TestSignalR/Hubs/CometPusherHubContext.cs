using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Primitives;

namespace TestSignalR.Hubs
{
    public abstract class CometPusherHubContext : HubBase
    {
        private readonly IUserResolver _userResolver;

        protected CometPusherHubContext(IUserResolver userResolver)
        {
            _userResolver = userResolver;
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (string.IsNullOrWhiteSpace(Context.ConnectionId))
            {
                return;
            }

            HttpContext httpContext = GetCurrentHttpContext();

            var cometContexts = GetCometContexts(httpContext);

            List<Task> tasks = new List<Task>();

            foreach (var cometContext in cometContexts)
            {
                tasks.Add(Task.Run(() => Groups.RemoveFromGroupAsync(Context.ConnectionId, cometContext)));
            }

            tasks.Add(Task.Run(() => Groups.RemoveFromGroupAsync(Context.ConnectionId, "all")));

            await Task.WhenAll(tasks);
        }

        public override async Task OnConnectedAsync()
        {
            if (string.IsNullOrWhiteSpace(Context.ConnectionId))
            {
                return;
            }

            HttpContext httpContext = GetCurrentHttpContext();

            var cometContexts = GetCometContexts(httpContext);
            

            List<Task> tasks = new List<Task>();
            

            foreach (var cometContext in cometContexts)
            {
                tasks.Add(Task.Run(() => Groups.AddToGroupAsync(Context.ConnectionId, cometContext)));
            }

            tasks.Add(Task.Run(() => Groups.AddToGroupAsync(Context.ConnectionId, "all")));

            await Task.WhenAll(tasks);
        }

        private List<string> GetCometContexts(HttpContext httpContext)
        {
            if (httpContext.Request.Query.TryGetValue("context", out StringValues cometContextValues))
            {
                return cometContextValues.ToList();
            }

            return new List<string>();
        }
        
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}