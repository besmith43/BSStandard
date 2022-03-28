﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MoneyTracker.Data;

#nullable disable

namespace MoneyTracker.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20211128022309_adding transactions and budget summaries")]
    partial class addingtransactionsandbudgetsummaries
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0");

            modelBuilder.Entity("MoneyTracker.Data.BudgetSummary", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("BudgetTotal")
                        .HasColumnType("TEXT");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Percent")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Spent")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("BudgetSummaries");
                });

            modelBuilder.Entity("MoneyTracker.Data.Transaction", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("PaymentSource")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<string>("Vendor")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
