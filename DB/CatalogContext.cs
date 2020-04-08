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
            // builder.AddProvider(new MyLoggerProvider());    // указываем наш провайдер логгирования
            builder.AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Debug)
                   .AddProvider(new MyLoggerProvider());
            //     builder.AddFilter((category, level) => category == DbLoggerCategory.Database.Connection.Name && level == LogLevel.Debug)
            //.AddProvider(new MyLoggerProvider());
        //    builder.AddFilter((category, level) => category == DbLoggerCategory.Database.Transaction.Name && level == LogLevel.Debug)
        //.AddProvider(new MyLoggerProvider());
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
                o.Property(v => v.PrtId).HasColumnName("PrtId");
                o.Property(v => v.Nnt).HasColumnName("Nnt");
                o.Property(v => v.Qnt).HasColumnName("Qnt");
                o.Property(v => v.SupInn).HasColumnName("SupInn");
                o.Property(v => v.Nds).HasColumnName("Nds");
                o.Property(v => v.PrcOptNds).HasColumnName("PrcOptNds");
                o.Property(v => v.PrcRet).HasColumnName("PrcRet");

            }));
            
            modelBuilder.Entity<Preorder>().HasKey(o => o.PreorderItemId);
            modelBuilder.Entity<Stock>().ToTable("Stocks").HasKey(o => o.StockItemId);
            modelBuilder.Entity<Store>().HasKey(o => o.StoreId);
            modelBuilder.Entity<OrderHeaderToStore>().ToTable("OrderHeader").HasKey(o => o.OrderHeaderId);
            modelBuilder.Entity<OrderRowToStore>().ToTable("OrderRows").HasKey(o => o.OrderRowId);
            modelBuilder.Entity<OrderStatusToStore>().ToTable("OrderStatus").HasKey(o => o.OrderStatusId);
            modelBuilder.Entity<ReservedStock>().ToTable("ReservedStocks").HasKey(o => o.ReservedStockItemId);

        }
    }
}
