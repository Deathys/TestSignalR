using System.Net;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRClient;

public class Program
{
    private static HubConnection connection;
    
    public static void Main(string[] args)
    {
        connection = new HubConnectionBuilder()
            .WithUrl("url",
                options =>
                {
                    options.Cookies.Add(new Cookie("token", "f53c5c19-bf37-463f-8037-f15146329164", "/", "domain"));
                })
            .WithAutomaticReconnect(new RandomRetryPolicy())
            .Build();
        
        connection.On<string, string>("ReceiveMessage", ReceivedMessage);
        
        connection.StartAsync().Wait();

        while (true)
        {
            Task.Delay(1000);
        }
        
        // using var signal = new EventWaitHandle(false, EventResetMode.AutoReset);
        //     
        // signal.WaitOne();
    }

    public static void ReceivedMessage(string user, string message)
    {
        Console.WriteLine($"{user}: {message}");
    }
}

public class RandomRetryPolicy : IRetryPolicy
{
    private readonly Random _random = new Random();

    public TimeSpan? NextRetryDelay(RetryContext retryContext)
    {
        // If we've been reconnecting for less than 60 seconds so far,
        // wait between 0 and 10 seconds before the next reconnect attempt.
        if (retryContext.ElapsedTime < TimeSpan.FromSeconds(60))
        {
            return TimeSpan.FromSeconds(_random.NextDouble() * 10);
        }
        else
        {
            // If we've been reconnecting for more than 60 seconds so far, stop reconnecting.
            return null;
        }
    }
}
