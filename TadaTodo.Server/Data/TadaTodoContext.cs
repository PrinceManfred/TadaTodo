using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TadaTodo.Server.Models;

namespace TadaTodo.Server.Data;

public class TadaTodoContext : DbContext, IDataProtectionKeyContext
{
    public TadaTodoContext(DbContextOptions<TadaTodoContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; init; }
    public DbSet<TodoList> TodoLists { get; init; }
    public DbSet<TodoItem> TodoItems { get; init; }

    public DbSet<DataProtectionKey> DataProtectionKeys { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(builder =>
        {
            builder.HasIndex(u => u.Username)
                .IsUnique();

            builder.HasMany(u => u.TodoLists)
                .WithOne()
                .IsRequired();
            
            builder.Property(u => u.Username)
                .HasMaxLength(50);
            
            builder.Property(u => u.Password)
                .HasMaxLength(1024);
        });

        modelBuilder.Entity<TodoList>(builder =>
        {
            builder.HasMany(tl => tl.TodoItems)
                .WithOne()
                .IsRequired();

            builder.Property(tl => tl.Name)
                .HasMaxLength(255);
        });
        
        modelBuilder.Entity<TodoItem>(builder =>
        {
            builder.Property(tl => tl.Value)
                .HasMaxLength(500);
        });
    }
}