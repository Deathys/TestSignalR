namespace TestSignalR.Hubs
{
    public class CommonCometPusherHubContext : CometPusherHubContext
    {
        public CommonCometPusherHubContext(IUserResolver userResolver) : base(userResolver)
        {
            
        }
    }
}