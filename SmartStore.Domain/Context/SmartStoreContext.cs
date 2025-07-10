using Microsoft.EntityFrameworkCore;
using SmartStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Domain.Context
{
    public class SmartStoreContext : DbContext
    {
        public SmartStoreContext(DbContextOptions<SmartStoreContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmartStoreContext).Assembly);
        }

        public object Find(long id)
        {
            throw new NotImplementedException();
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<ItemType> ItemGroups { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<ItemUnit> ItemUnits { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreItemQuantity> StoreItemQuantities { get; set; }
        public DbSet<StockAdjustment> StockAdjustments { get; set; }
        public DbSet<DamagedItem> DamagedItems { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Revenue> Revenues { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<Safe> Safes { get; set; }
        public DbSet<SafeTransaction> SafeTransactions { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }

    }
}
