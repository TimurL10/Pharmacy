using WorkWithFarmacy.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace WorkWithFarmacy.DB
{
    public class CatalogContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        }

        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddProvider(new MyLoggerProvider());    // указываем наш провайдер логгирования
            builder.AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Debug)
                   .AddProvider(new MyLoggerProvider());
            builder.AddFilter((category, level) => category == DbLoggerCategory.Database.Connection.Name && level == LogLevel.Debug)
       .AddProvider(new MyLoggerProvider());
            builder.AddFilter((category, level) => category == DbLoggerCategory.Database.Transaction.Name && level == LogLevel.Debug)
        .AddProvider(new MyLoggerProvider());
        });

        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated(); 
        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Preorder> Preorders { get; set; }
        public DbSet<OrderRowToStore> OrderRows { get; set; }
        public DbSet<OrderHeaderToStore> OrderHeader { get; set; }
        public DbSet<OrderStatusToStore> OrderStatus { get; set; }         
        public DbSet<PostStock> FullStock { get; set; }
        public DbSet<OrderRowToStore> ReservedRows { get; set; }
        public DbSet<ReservedStock> ReservedStocks { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }
            modelBuilder.Entity<PostStock>((o =>
            {
                o.HasNoKey();                
                o.ToView("view_fullstock");
            }));
            
            modelBuilder.Entity<Preorder>().HasKey(o => o.PreorderItemId);
            modelBuilder.Entity<Stock>().ToTable("stocks").HasKey(o => o.StockItemId);
            modelBuilder.Entity<Store>().HasKey(o => o.StoreId);
            modelBuilder.Entity<OrderHeaderToStore>().ToTable("orderHeader").HasKey(o => o.OrderHeaderId);
            modelBuilder.Entity<OrderRowToStore>().ToTable("orderRows").HasKey(o => o.OrderRowId);
            modelBuilder.Entity<OrderStatusToStore>().ToTable("orderStatus").HasKey(o => o.OrderStatusId);
            modelBuilder.Entity<OrderRowToStore>().ToTable("reservedRows").HasKey(o => o.OrderRowId);
            modelBuilder.Entity<ReservedStock>().ToTable("reservedstocks").HasKey(o => o.ReservedStockItemId);

        }
    }
}
