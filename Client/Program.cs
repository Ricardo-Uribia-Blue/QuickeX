using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using QuickEx.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddTransient(sp => new HttpClient{BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});
builder.Services.AddScoped<QuickEx.Client.QuickPruebaService>();
var host = builder.Build();
await host.RunAsync();