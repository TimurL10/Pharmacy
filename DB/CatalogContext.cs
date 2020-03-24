using WorkWithFarmacy.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace WorkWithFarmacy.DB
{
    public class CatalogContext : DbContext
    {        
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
           // Database.EnsureDeleted();
            Database.EnsureCreated(); 
        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Preorder> Preorders { get; set; }
        public DbSet<OrderRowToStore> OrderRows { get; set; }
        public DbSet<OrderHeaderToStore> OrderHeader { get; set; }
        public DbSet<OrderStatusToStore> OrderStatus { get; set; }         
        public DbSet<PostStock> FullStock { get; set; }
        public DbSet<ReservedRows> ReservedRows { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }
            modelBuilder.Entity<PostStock>((o =>
            {
                o.HasNoKey();
                o.ToView("View_FullStock", "PostStock");
            }));
            
            modelBuilder.Entity<Preorder>().HasKey(o => o.PreorderItemId);
            modelBuilder.Entity<Stock>().ToTable("Stocks").HasKey(o => o.StockItemId);
            modelBuilder.Entity<Store>().HasKey(o => o.StoreId);
            modelBuilder.Entity<OrderHeaderToStore>().ToTable("OrderHeader").HasKey(o => o.OrderHeaderId);
            modelBuilder.Entity<OrderRowToStore>().ToTable("OrderRows").HasKey(o => o.OrderRowId);
            modelBuilder.Entity<OrderStatusToStore>().ToTable("OrderStatus").HasKey(o => o.OrderStatusId);
            modelBuilder.Entity<ReservedRows>().ToTable("ReservedRows").HasKey(o => o.OrderRowId);
        }
    }
}
