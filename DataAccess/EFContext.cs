using DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class EFContext: IdentityDbContext <User>
    {
        public EFContext(DbContextOptions<EFContext> options): base(options){}
        
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Pizza> Pizzas { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Direction> Directions { get; set; }
    }
}