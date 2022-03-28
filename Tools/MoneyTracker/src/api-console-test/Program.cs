using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

RunAsync().Wait();

static async Task RunAsync()
{
    Console.WriteLine("Starting Async Method");
    
    var handler = new HttpClientHandler();
    handler.ClientCertificateOptions = ClientCertificateOption.Manual;
    handler.ServerCertificateCustomValidationCallback =
        (httpRequestMessage, ClientCertificateOption, cetChain, policyErrors) =>
        { return true; };
    
   using (var client = new HttpClient(handler))
   {
      BudgetSummary[] BudgetItems = await client.GetFromJsonAsync<BudgetSummary[]>("https://localhost:7121/Budget");

      Console.WriteLine("-------------------------------");
      foreach (var item in BudgetItems)
      {
          Console.WriteLine($"Category: { item.Category }\nBudget total: { item.BudgetTotal }\nSpent: { item.Spent }\nPercent: { item.Percent }"); 
          Console.WriteLine("-------------------------------");
      }
   }

   Console.WriteLine("Ending Async Method");
}

public class BudgetSummary
{
    public int ID { get; set; }
    public string Category { get; set; }
    public decimal BudgetTotal { get; set; }
    public decimal Spent { get; set; }
    public int Percent { get; set; }
}