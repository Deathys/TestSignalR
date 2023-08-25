using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.SignalR;

namespace TestSignalR
{
    public static class SignalrInitializer
    {
        public static ISignalRServerBuilder AddCometPusher(this IServiceCollection services, string redisConnectionString)
        {
            return services
                .AddSignalR(options =>
                {
                    options.EnableDetailedErrors = true;
                    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
                })
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerOptions.IgnoreReadOnlyProperties = false;
                    
                    options.PayloadSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                    
                    options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    
                    options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                    options.PayloadSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                })
                .AddStackExchangeRedis(redisConnectionString, options => {
                    options.Configuration.ChannelPrefix = "MyApp";
                });
        }
    }
}