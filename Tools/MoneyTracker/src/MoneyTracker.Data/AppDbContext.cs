using System;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Class;

namespace MoneyTracker.Data;
public class AppDbContext: DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // this connection string value will have to be changed eventually as it only works in dev
        var connString = "Data Source=../MoneyTracker.Data/app.db";
        optionsBuilder.UseSqlite(connString);

        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<BudgetSummary> BudgetSummaries { get; set; }
}
