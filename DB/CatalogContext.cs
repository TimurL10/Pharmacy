using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkWithFarmacy.Models;

namespace WorkWithFarmacy.DB
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
            
        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Preorder> Preorders { get; set; }
        public DbSet<OrderHeaderToStore> OrderHeadersList { get; set; }
        public DbSet<OrderRowToStore> OrderRowsList { get; set; }
        public DbSet<OrderStatusToStore> OrderStatusesList { get; set; }
    }
}
