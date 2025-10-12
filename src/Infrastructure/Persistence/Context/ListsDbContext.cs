using ListService.Domain.Entities;
using ListService.Domain.Enums;
using ListService.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace ListService.Infrastructure.Persistence.Context;

public class ListsDbContext : DbContext
{
    public ListsDbContext(DbContextOptions<ListsDbContext> options) : base(options) {} 
    
    public DbSet<ShoppingList> ShoppingLists => Set<ShoppingList>();
    public DbSet<ListItem> ListItems => Set<ListItem>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<ShareLink> ShareLinks => Set<ShareLink>();
    public DbSet<ListChange> ListChanges => Set<ListChange>();
    public DbSet<BatchOperation> BatchOperations => Set<BatchOperation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<ChangeType>(null, "change_type")
            .HasPostgresEnum<SharePermissionType>(null, "share_permission_type")
            .HasPostgresEnum<BatchStatus>(null, "batch_status");
        

        modelBuilder.ApplyConfiguration(new ShoppingListConfig());
        modelBuilder.ApplyConfiguration(new ListItemConfig());
        modelBuilder.ApplyConfiguration(new CategoryConfig());
        modelBuilder.ApplyConfiguration(new ShareLinkConfig());
        modelBuilder.ApplyConfiguration(new ListChangeConfig());
        modelBuilder.ApplyConfiguration(new BatchOperationConfig());
        
        modelBuilder.Entity<ShoppingList>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<ListItem>().HasQueryFilter(x => !x.IsDeleted);
    }
}