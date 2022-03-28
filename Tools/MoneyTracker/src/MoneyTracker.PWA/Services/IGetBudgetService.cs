using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyTracker.Class;


namespace MoneyTracker.PWA.Services;

public interface IGetBudgetService
{
    Task<IEnumerable<BudgetSummary>> GetAllItems();
    Task<BudgetSummary> GetItemDetails(int id);
}