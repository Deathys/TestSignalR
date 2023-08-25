using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TestSignalR.Hubs;

namespace TestSignalR;

public class Startup
{
    
    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        var mvcBuilder = services.AddMvcCore(options => {
                options.EnableEndpointRouting = false;
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            })
            .AddApiExplorer(); // для работы сваггера нужно
        
        services.AddHttpClient();
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            // c.SchemaFilter<SwaggerSchemaParametersFilter>();
            c.SchemaGeneratorOptions.SchemaIdSelector = type => $"{type.Namespace}.{type.Name}";
        });
        
        //services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        
        // Add services to the container.
        services.AddRazorPages();
        services.AddCometPusher("127.0.0.1:6379,abortConnect=false,connectTimeout=1000,password=1");
        
        return services.BuildServiceProvider();
    }
    

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        
        app.UseMvc(builder =>
        {
        });
        
        // Configure the HTTP request pipeline.
        if(env.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseDefaultFiles();
        
        app.UseFileServer(
            new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
            });
        
        app.UseRouting();

        //app.UseAuthorization();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<CommonCometPusherHubContext>("/pub/comet", options =>
            {
                options.Transports =
                    HttpTransportType.WebSockets | HttpTransportType.LongPolling | HttpTransportType.ServerSentEvents;
            });
            endpoints.MapRazorPages();
        });
    }
}