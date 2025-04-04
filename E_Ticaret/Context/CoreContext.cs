using E_Ticaret.Domains.Entities;
using E_Ticaret.Domains.Entities.Base.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace E_Ticaret.Contexts
{
    public class CoreContext : DbContext
    {
        public CoreContext(DbContextOptions<CoreContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet(CharSet.Utf8Mb4, true)
           .UseCollation("utf8mb4_turkish_ci");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CartItem>()
           .HasOne(ci => ci.Cart)
           .WithMany(c => c.CartItems)
           .HasForeignKey(ci => ci.CartId)
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(10,2)");

            // CategoryRelation configuration
            modelBuilder.Entity<CategoryRelation>()
                .HasKey(cr => new { cr.ParentCategoryId, cr.ChildCategoryId });

            modelBuilder.Entity<CategoryRelation>()
                .HasOne(cr => cr.ParentCategory)
                .WithMany(c => c.ChildRelations)
                .HasForeignKey(cr => cr.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CategoryRelation>()
                .HasOne(cr => cr.ChildCategory)
                .WithMany(c => c.ParentRelations)
                .HasForeignKey(cr => cr.ChildCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // ProductCategory configuration
            modelBuilder.Entity<ProductCategory>()
                .HasKey(pc => new { pc.ProductId, pc.CategoryId });

            modelBuilder.Entity<ProductCategory>()
            .HasOne(pc => pc.Category)
            .WithMany(c => c.ProductCategories)
            .HasForeignKey(pc => pc.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // ProductSize configuration
            modelBuilder.Entity<ProductSize>()
                .HasKey(ps => new { ps.ProductId, ps.SizeId });

            modelBuilder.Entity<ProductSize>()
                .HasOne(ps => ps.Product)
                .WithMany(p => p.ProductSizes)
                .HasForeignKey(ps => ps.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductSize>()
                .HasOne(ps => ps.Size)
                .WithMany(s => s.ProductSizes)
                .HasForeignKey(ps => ps.SizeId)
                .OnDelete(DeleteBehavior.Cascade);

            // ProductColor configuration
            modelBuilder.Entity<ProductColor>()
                .HasKey(pc => new { pc.ProductId, pc.ColorId });

            modelBuilder.Entity<ProductColor>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductColors)
                .HasForeignKey(pc => pc.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductColor>()
                .HasOne(pc => pc.Color)
                .WithMany(c => c.ProductColors)
                .HasForeignKey(pc => pc.ColorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product price column configuration
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(10,2)");
        }

        public async Task<int> SaveCoreContextAsync(Int64 userId, DateTimeOffset saveDate)
        {
            var transId = -1;
            if (this.ChangeTracker.HasChanges())
            {
                using (var dbContextTransaction = this.Database.BeginTransaction())
                {
                    try
                    {
                        if (this != null)
                        {
                            InsertAuthenticationBaseEntity(this, userId, saveDate);
                            UpdateAuthenticationBaseEntity(this, userId, saveDate);
                            DeleteAuthenticationBaseEntity(this, userId);
                            transId = await base.SaveChangesAsync();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        throw new Exception(ex.ToString());
                    }
                }
            }
            else
            {
                transId = 0;
            }
            return transId;
        }

        private void InsertAuthenticationBaseEntity(DbContext dbContext, Int64 userId, DateTimeOffset timeStamp)
        {
            foreach (var entity in dbContext.ChangeTracker.Entries<BaseEntity>().Where(x => x.State == EntityState.Added).ToList())
            {
                var dd = default(DateTimeOffset).AddDays(1);
                var dateTime = timeStamp.ToString();
                if (entity.Entity.CreatedAt < dd)
                    entity.Entity.CreatedAt = Convert.ToDateTime(dateTime);

                entity.Entity.IsDeleted = false;
                entity.Entity.IsActive = true;
            }
        }
        private void UpdateAuthenticationBaseEntity(DbContext dbContext, Int64 userId, DateTimeOffset timeStamp)
        {
            foreach (var entity in dbContext.ChangeTracker.Entries<BaseEntity>().Where(x => x.State == EntityState.Modified).ToList())
            {
                var dateTime = timeStamp.ToString();
                entity.Entity.UpdatedAt = Convert.ToDateTime(dateTime);
            }
        }

        private void DeleteAuthenticationBaseEntity(DbContext dbContext, Int64 userId)
        {
            foreach (var entity in dbContext.ChangeTracker.Entries<BaseEntity>().Where(x => x.State == EntityState.Deleted).ToList())
            {
                var dateTime = DateTimeOffset.Now.ToString();
                entity.Entity.IsDeleted = true;
                entity.Entity.IsActive = false;
                entity.Entity.UpdatedAt = Convert.ToDateTime(dateTime);
                entity.State = EntityState.Modified;
            }
        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<CategoryRelation> CategoryRelations { get; set; }
        
    }
}
