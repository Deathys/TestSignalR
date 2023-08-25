namespace TestSignalR
{
    public class Program
    {
        public static IWebHost? WebHost { get; set; }
        
        public static void Main(string[] args)
        {
            WebHost = new WebHostBuilder()
                .UseKestrel(options => { options.Limits.MaxRequestBodySize = 256 * (1024 * 1024); })
                .UseStartup<Startup>()
                .Build();

            WebHost.Run();
        }
    }
}

// var builder = WebApplication.CreateBuilder(args);
//
// var app = builder.Build();

// // Configure the HTTP request pipeline.

//
// app.Run();
//