using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using UI.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7237") });

await builder.Build().RunAsync();