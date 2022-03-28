using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MoneyTracker.Class;

namespace MoneyTracker.PWA.Services;

public class GetBudgetService : IGetBudgetService
{
    private readonly HttpClient _httpClient;

    public GetBudgetService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<BudgetSummary>> GetAllItems()
    {
        var apiResponse = await _httpClient.GetStreamAsync($"Budget");
        return await JsonSerializer.DeserializeAsync<IEnumerable<BudgetSummary>>(apiResponse,
            new JsonSerializerOptions() {PropertyNameCaseInsensitive = true});
    }

    public async Task<BudgetSummary> GetItemDetails(int id)
    {
        var apiResponse = await _httpClient.GetStreamAsync($"Budget/{id}");
        return await JsonSerializer.DeserializeAsync<BudgetSummary>(apiResponse,
            new JsonSerializerOptions() {PropertyNameCaseInsensitive = true});
    }
}