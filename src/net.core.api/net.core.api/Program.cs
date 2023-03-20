using Microsoft.Extensions.Options;
using net.core.api;
using OpenAI.GPT3.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenAIService(settings => { settings.ApiKey = "YOUR API KEY HERE"; });
builder.Services.AddSignalR();
builder.Services.AddCors(p => p.AddPolicy("cors", builder =>
{
    builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
}));

var app = builder.Build();
app.UseCors("cors"); 
app.UseHttpsRedirection();
app.MapHub<ChatHub>("/chat");

app.Run();
