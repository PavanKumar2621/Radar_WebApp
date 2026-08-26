using WebSocketDemo.Hubs;
using WebSocketDemo.Handlers;
using WebSocketDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Razor Pages
builder.Services.AddRazorPages();

// Add SignalR
builder.Services.AddSignalR();

builder.Services.AddSingleton<MessageRouter>();
builder.Services.AddSingleton<CommandHandler>();
builder.Services.AddSingleton<ServoService>();
builder.Services.AddSingleton<RadarService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

// SignalR Hub
app.MapHub<CommunicationHub>("/communicationHub");

app.Run();