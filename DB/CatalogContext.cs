using WorkWithFarmacy.Models;
using Microsoft.EntityFrameworkCore;



namespace WorkWithFarmacy.DB
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
            Database.EnsureCreated(); 
        }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Preorder> Preorders { get; set; }
        public DbSet<OrderHeaderToStore> OrderHeadersList { get; set; }
        public DbSet<OrderRowToStore> OrderRowsList { get; set; }
        public DbSet<OrderStatusToStore> OrderStatusesList { get; set; }
        public DbSet<PutOrderToSite> FullOrdersList { get; set; }
        
        //ModelBuilder ModelBuilder = new ModelBuilder()

        //protected override void onModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<OrderHeaderToStore>().HasKey(o => o.OrderId);
        //    modelBuilder.Entity<OrderRowToStore>().HasKey(o => o.RowId);
        //    modelBuilder.Entity<OrderStatusToStore>().HasKey(o => o.StatusId);
        //}
    }
}
