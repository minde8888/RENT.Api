using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RENT.Domain.Entities;
using RENT.Domain.Entities.Auth;
using RENT.Domain.Entities.Roles;

namespace RENT.Data.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        public DbSet<IdentityUserClaim<Guid>> IdentityUserClaims { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Posts> Posts { get; set; }
        public DbSet<Seller> Seller { get; set; }
        public DbSet<Shop> Shop { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<CategoriesProduct> CategoriesProduct { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Core Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Core Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.HasDefaultSchema("Identity");
            //Global Query Filters
            builder.Entity<Categories>().HasQueryFilter(p => p.IsDeleted == false);
            builder.Entity<Customers>().HasQueryFilter(p => p.IsDeleted == false);
            builder.Entity<Address>().HasQueryFilter(p => p.IsDeleted == false);
            builder.Entity<Products>().HasQueryFilter(p => p.IsDeleted == false);
            builder.Entity<Posts>().HasQueryFilter(p => p.IsDeleted == false);
            builder.Entity<Seller>().HasQueryFilter(p => p.IsDeleted == false);
            builder.Entity<Shop>().HasQueryFilter(p => p.IsDeleted == false);
            builder.Entity<ApplicationUser>().HasQueryFilter(p => p.IsDeleted == false);

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("UserLogins");
            });

            builder.Entity<CategoriesProduct>()
                .HasKey(i => new { i.ProductsId, i.CategoriesId });

            builder.Entity<Categories>()
           .HasMany(x => x.Products)
           .WithMany(x => x.Categories)
           .UsingEntity<CategoriesProduct>(
               x => x.HasOne(x => x.Products)
               .WithMany().HasForeignKey(x => x.ProductsId),
               x => x.HasOne(x => x.Categories)
              .WithMany().HasForeignKey(x => x.CategoriesId));

            builder.Entity<Customers>()
            .HasOne(b => b.Address)
            .WithOne(i => i.Customers)
            .HasForeignKey<Address>(b => b.CustomerId);

            builder.Entity<Customers>()
            .HasMany(c => c.Products)
            .WithOne(e => e.Customers);

            builder.Entity<Seller>()
            .HasOne(b => b.Address)
            .WithOne(i => i.Seller)
            .HasForeignKey<Address>(b => b.SellerId);

            builder.Entity<Seller>()
            .HasMany(c => c.Products)
            .WithOne(e => e.Seller);


            builder.Entity<Shop>()
            .HasOne(b => b.Address)
            .WithOne(i => i.Shop)
            .HasForeignKey<Address>(b => b.ShopId);

            builder.Entity<Products>()
             .HasOne(c => c.Posts)
             .WithOne(e => e.Products)
              .HasForeignKey<Posts>(b => b.ProductsId);
        }
    }
}