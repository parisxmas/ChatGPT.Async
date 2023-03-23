using System.Reflection;
using Microsoft.Extensions.Options;
using net.core.api;
using net.core.api.Middlewares;
using net.core.business.Caching.Base;
using net.core.business.Caching.Redis.Server;
using net.core.business.Caching.Redis.Service;
using net.core.business.DataAccessFolder;
using net.core.business.Services.ChatServiceFolder;
using net.core.business.Services.SendersServiceFolder;
using net.core.data.POCOs;
using OpenAI.GPT3.Extensions;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

builder.Services.AddOpenAIService(settings => { settings.ApiKey = configuration["OPEN_API_KEY"]; });
builder.Services.AddSignalR();
builder.Services.AddCors(p => p.AddPolicy("cors", builder =>
{
    builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
}));

builder.Services.AddTransient<IChatService, ChatService>();
builder.Services.AddTransient<IConn, Conn>();
builder.Services.AddTransient<ISenderService, SenderService>();
builder.Services.AddTransient<ICacheService, RedisCacheService>();
builder.Services.AddSingleton<RedisServer>();

#region Logging Configuration
ConfigureLogging();
builder.Host.UseSerilog();
#endregion



var app = builder.Build();
app.UseCors("cors"); 
app.UseHttpsRedirection();
app.MapHub<ChatHub>("/chat");

app.UseMiddleware<GlobalErrorHandlerMiddleware>();

app.Run();



void ConfigureLogging()
{
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile(
            $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
            optional: true)
        .Build();

    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
        .Enrich.WithProperty("Environment", environment)
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}

ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
{
    return new ElasticsearchSinkOptions(new Uri(configuration.GetSection("ElasticConfiguration:Uri").Value))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
    };
}

