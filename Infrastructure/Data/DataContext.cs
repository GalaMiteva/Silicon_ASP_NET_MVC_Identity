﻿

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<UserEntity>(options)
{
    public DbSet<AddressEntity> Addresses { get; set; } = null!;

    //protected override void OnModelCreating(ModelBuilder builder)
    //{
    //    base.OnModelCreating(builder);

    //    builder.Entity<UserEntity>()
    //    .HasMany(u => u.Address)
    //    .WithOne(a=>a.User)
    //    .HasForeignKey(a=>a.UserId)
    //    .OnDelete(DeleteBehavior.Restrict);

    //}

    public DbSet<UserEntity> Users { get; set; } = null!;

    public DbSet<FeatureEntity> Features { get; set; }
    public DbSet<FeatureItemEntity> FeaturesItems { get; set; }
}