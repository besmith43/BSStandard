using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Class;
using MoneyTracker.Data;

namespace MoneyTracker.Data;

public class DbActions
{
    private AppDbContext db = new AppDbContext();
    private bool hasZeroedOut = false;

    public IEnumerable<BudgetSummary> GetBudgetSummaries()
    {
        // need to evaluate the first of a new month to zero out the budget summary
        if (IsFirstOfTheMonth())
        {
            ZeroOutSummary();
        }
        else
        {
            hasZeroedOut = false;
        }
        
        return db.BudgetSummaries;
    }

    private bool IsFirstOfTheMonth()
    {
        DateTime dt = DateTime.Today;

        if (dt.Day == 1 && !hasZeroedOut)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ZeroOutSummary()
    {
        hasZeroedOut = true;

        var currentBudgetSummary = db.BudgetSummaries;

        foreach (var item in currentBudgetSummary)
        {
            item.Spent = 0.0M;
            item.Percent = 0;
        }

        db.SaveChanges();
    }

    /*
    public List<BudgetSummary> GetBudgetSummaryList()
    {
        return db.BudgetSummaries.AsQueryable().ToList();
    }
    */
    
    private void UpdateBudgetSummary(Transaction newPurchase)
    {
        var budgetSummaryQuery = db.BudgetSummaries.Where(bs => bs.Category.Equals(newPurchase.Category)).ToList();

        if (budgetSummaryQuery[0].BudgetTotal > 0)
        {
            budgetSummaryQuery[0].Spent += newPurchase.Price;
            budgetSummaryQuery[0].Percent =
                Decimal.ToInt32(budgetSummaryQuery[0].Spent / budgetSummaryQuery[0].BudgetTotal * 100);
        }
        else
        {
            budgetSummaryQuery[0].Spent += newPurchase.Price;
            budgetSummaryQuery[0].Percent =
                Decimal.ToInt32(budgetSummaryQuery[0].Spent * 100);
        }

        // db.SaveChanges();
    }
    
    public void AddTransaction(Transaction newPurchase)
    {
        UpdateBudgetSummary(newPurchase);
        db.Transactions.Add(newPurchase);
        db.SaveChanges();
    }
}
