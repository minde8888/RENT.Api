﻿using Microsoft.AspNetCore.Identity;
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

        public DbSet<Categories> Categories { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Seller> Seller { get; set; }
        public DbSet<Shop> Shop { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<ProductsCategories> ProductsCategories { get; set; }

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
            builder.Entity<Seller>().HasQueryFilter(p => p.IsDeleted == false);
            builder.Entity<Shop>().HasQueryFilter(p => p.IsDeleted == false);
            builder.Entity<ApplicationUser>().HasQueryFilter(p => p.IsDeleted == false);

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("UserLogins");
            });

            builder.Entity<ProductsCategories>()
                .HasKey(i => new { i.ProductsId, i.CategoriesId });

            builder.Entity<Categories>()
           .HasMany(x => x.Products)
           .WithMany(x => x.Categories)
           .UsingEntity<ProductsCategories>(
               x => x.HasOne(x => x.Products)
               .WithMany().HasForeignKey(x => x.ProductsId),
               x => x.HasOne(x => x.Categories)
              .WithMany().HasForeignKey(x => x.CategoriesId));

            builder.Entity<Customers>()
            .HasOne(b => b.Address)
            .WithOne(i => i.Customers)
            .HasForeignKey<Customers>(b => b.Id);

            builder.Entity<Seller>()
            .HasOne(b => b.Address)
            .WithOne(i => i.Seller)
            .HasForeignKey<Seller>(b => b.Id);

            builder.Entity<Shop>()
            .HasOne(b => b.Address)
            .WithOne(i => i.Shop)
            .HasForeignKey<Shop>(b => b.ShopId);
        }
    }
}