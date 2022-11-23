using DocumentSearcher.Services;
using Serilog.Events;
using Serilog;
using Nest;
using Elasticsearch.Net;
using Serilog.Sinks.LogstashHttp;

namespace DocumentSearcher
{
    public class Program
    {
        public static int Main(string[] args)
        {
            LogstashHttpSinkOptions options = new LogstashHttpSinkOptions 
            { 
                InlineFields = true,
                MinimumLogEventLevel = LogEventLevel.Information,
                LogstashUri = "https://localhost:8443"
            };
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .WriteTo.Console()
                .Enrich.WithProperty("ApplicationName", "Serilog.Sinks.LogstashHttp.ExampleApp")
                .WriteTo.LogstashHttp(options)
                .CreateLogger();
            try
            {
                var builder = WebApplication.CreateBuilder(args);
                builder.Host.UseSerilog();
                Configure(builder);
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        static void Configure(WebApplicationBuilder builder)
        {
            Log.Information("Starting web host");
            
            
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddTransient<SearchService>();
            builder.Services.AddSingleton<IElasticClient>(sp =>
            {
                var cloudId = "Learning:dXMtY2VudHJhbDEuZ2NwLmNsb3VkLmVzLmlvOjQ0MyRlZmU2Y2UwMGQwY2U0ZDA1YjgwZDBiMDBmMWE3MDliOCQ0NzY4OWMxZjU4YTc0NTBkOTQ5OTVjOGU2NThlYzE0OQ==";
                var credentials = new BasicAuthenticationCredentials("elastic", "riNNe9R6GkPPnSuDaHxwEjXx");
                var settings = new ConnectionSettings(cloudId, credentials);
                settings.DefaultFieldNameInferrer(p => p);
                return new ElasticClient(settings);
            });
            builder.Services.AddGrpc();

            var app = builder.Build();
            app.UseSerilogRequestLogging();
            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGrpcService<GreeterService>();

            app.MapControllers();

            app.Run();
        }
    }
}