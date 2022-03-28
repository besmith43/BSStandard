using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyTracker.Data.Migrations
{
    public partial class addingtransactionsandbudgetsummaries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BudgetSummaries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    BudgetTotal = table.Column<decimal>(type: "TEXT", nullable: false),
                    Spent = table.Column<decimal>(type: "TEXT", nullable: false),
                    Percent = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetSummaries", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Vendor = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    PaymentSource = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "BudgetSummaries",
                columns: new[] { "ID", "Category", "BudgetTotal", "Spent", "Percent" },
                values: new object[] { 1, "Groceries", 350, 113.87, 33 }
            );

            migrationBuilder.InsertData(
                table: "BudgetSummaries",
                columns: new[] { "ID", "Category", "BudgetTotal", "Spent", "Percent" },
                values: new object[] { 2, "Toys", 0, 4.23, 0 }
            );

            migrationBuilder.InsertData(
                table: "BudgetSummaries",
                columns: new[] { "ID", "Category", "BudgetTotal", "Spent", "Percent" },
                values: new object[] { 3, "Food", 150, 66.62, 44}
            );

            migrationBuilder.InsertData(
                table: "BudgetSummaries",
                columns: new[] { "ID", "Category", "BudgetTotal", "Spent", "Percent" },
                values: new object[] { 4, "Rent", 510, 510, 100 }
            );

            migrationBuilder.InsertData(
                table: "BudgetSummaries",
                columns: new[] { "ID", "Category", "BudgetTotal", "Spent", "Percent" },
                values: new object[] { 5, "Electric", 100, 135, 135 }
            );

            migrationBuilder.InsertData(
                table: "BudgetSummaries",
                columns: new[] { "ID", "Category", "BudgetTotal", "Spent", "Percent" },
                values: new object[] { 6, "Gas", 60, 40.69, 68 }
            );

            migrationBuilder.InsertData(
                table: "BudgetSummaries",
                columns: new[] { "ID", "Category", "BudgetTotal", "Spent", "Percent" },
                values: new object[] { 7, "Loan", 160, 158, 99 }
            );

            migrationBuilder.InsertData(
                table: "BudgetSummaries",
                columns: new[] { "ID", "Category", "BudgetTotal", "Spent", "Percent" },
                values: new object[] { 8, "Life Insurance", 140, 140, 100 }
            );

            migrationBuilder.InsertData(
                table: "BudgetSummaries",
                columns: new[] { "ID", "Category", "BudgetTotal", "Spent", "Percent" },
                values: new object[] { 9, "Car Insurance", 120, 0, 0 }
            );

            migrationBuilder.InsertData(
                table: "BudgetSummaries",
                columns: new[] { "ID", "Category", "BudgetTotal", "Spent", "Percent" },
                values: new object[] { 10, "Internet", 95, 95, 100 }
            );

            migrationBuilder.InsertData(
                table: "BudgetSummaries",
                columns: new[] { "ID", "Category", "BudgetTotal", "Spent", "Percent" },
                values: new object[] { 11, "Cellphone", 35, 30.39, 87 }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetSummaries");

            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
