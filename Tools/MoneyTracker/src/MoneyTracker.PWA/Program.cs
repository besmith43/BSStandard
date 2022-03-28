using System;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Http;
using Blazored.Modal;
using Microsoft.Extensions.DependencyInjection;
using MoneyTracker.PWA;
using MoneyTracker.PWA.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// eventually this will need to be removed
/*var handler = new HttpClientHandler();
handler.ClientCertificateOptions = ClientCertificateOption.Manual;
handler.ServerCertificateCustomValidationCallback =
    (httpRequestMessage, ClientCertificateOption, cetChain, policyErrors) =>
    { return true; };

builder.Services.AddScoped(sp => new HttpClient(handler)
{
    //BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    BaseAddress = new Uri("https://localhost:7121")
});
*/

// got this from https://dev.to/moe23/blazor-wasm-with-rest-api-step-by-step-2djo
builder.Services.AddHttpClient<IGetBudgetService, GetBudgetService>(x =>
    x.BaseAddress = new Uri("http://localhost:7121"));

builder.Services.AddBlazoredModal();

await builder.Build().RunAsync();
