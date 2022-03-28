using Microsoft.AspNetCore.Mvc;
using MoneyTracker.Data;
using MoneyTracker.Class;

namespace MoneyTracker.API.Controllers;

[ApiController]
[Route("[controller]")]
public class BudgetController : ControllerBase
{
    private readonly ILogger<BudgetController> _logger;
    private DbActions dbActions = new DbActions();

    public BudgetController(ILogger<BudgetController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetMonthlyBudgetSummary")]
    public IEnumerable<BudgetSummary> Get()
    {
        return dbActions.GetBudgetSummaries();
    }

    [HttpPost(Name = "AddNewTransaction")]
    public void Put(Transaction newPurchase)
    {
            dbActions.AddTransaction(newPurchase);
    }
}