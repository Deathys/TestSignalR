using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace TestSignalR.Hubs
{
    public class UserIdProvider : IUserIdProvider
    {
        static volatile ConcurrentDictionary<string, byte> _badTokens = new ();
        static volatile ConcurrentDictionary<string, Guid> _goodTokens = new ();
        static DateTime _ttl = DateTime.MinValue;

        public UserIdProvider()
        {
        }
        
        public string GetUserId(HubConnectionContext connection)
        {
            var httpContext = connection.GetHttpContext();

            var (userId, _) = GetUserIdAndToken(httpContext).ConfigureAwait(false).GetAwaiter().GetResult();

            return userId;
        }

        public string GetUserContextId(string cometContext, string userId)
        {
            return $"{cometContext}:{userId}";
        }

        private Task<(string userId, string token)> GetUserIdAndToken(HttpContext context)
        {
            return Task.FromResult((Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));
        }
    }
}