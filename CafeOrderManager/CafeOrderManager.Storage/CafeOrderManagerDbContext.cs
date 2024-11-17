//using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dbo;
using Microsoft.EntityFrameworkCore;

namespace CafeOrderManager.Storage
{
    public class CafeOrderManagerDbContext : DbContext
    {
        public CafeOrderManagerDbContext(DbContextOptions<CafeOrderManagerDbContext> options) : base(options)
        {
        }

        #region DbSet
        public virtual DbSet<TableDbo> Table { get; set; }
        public virtual DbSet<UserDbo> User { get; set; }
        public virtual DbSet<CategoryDbo> Category { get; set; }
        public virtual DbSet<ProductDbo> Product { get; set; }
        public virtual DbSet<OrderDbo> Order { get; set; }
        public virtual DbSet<OrderItemDbo> OrderItem { get; set; }
        public virtual DbSet<PaymentDbo> Payment { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Category - Product Relationship

            modelBuilder.Entity<CategoryDbo>()
        .HasMany(c => c.Products)
        .WithOne(p => p.Category)
        .HasForeignKey(p => p.CategoryId)
        .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Product - OrderItem Relationship

            modelBuilder.Entity<ProductDbo>()
                .HasMany(p => p.OrderItems)
                .WithOne(oi => oi.Product)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Ürün silinirse, siparişler etkilenmesin.

            #endregion

            #region Order - OrderItem Relationship

            modelBuilder.Entity<OrderDbo>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Eğer sipariş silinirse, sipariş öğeleri de silinsin.

            #endregion

            #region Order - Table Relationship

            modelBuilder.Entity<OrderDbo>()
                .HasOne(o => o.Table)
                .WithMany(t => t.Orders)
                .HasForeignKey(o => o.TableId)
                .OnDelete(DeleteBehavior.Restrict); // Masa silinirse, siparişler etkilenmesin.

            #endregion

            #region Order - Payment Relationship

            modelBuilder.Entity<OrderDbo>()
                .HasMany(o => o.Payments)
                .WithOne(p => p.Order)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Eğer sipariş silinirse, ödemeler de silinsin.

            #endregion


        }

    }
}