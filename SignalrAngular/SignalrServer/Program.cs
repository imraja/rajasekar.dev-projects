using Microsoft.AspNetCore.SignalR;
using SignalrServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// Add SignalR
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", b => b
        .WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.MapHub<MessageHub>("latest-messages");

app.MapGet("/send-messages", (IHubContext<MessageHub> notificationsHub) => notificationsHub.Clients.All.SendAsync("Receive-Messages",$"Message from server - {DateTime.Now}!"));

app.Run();
