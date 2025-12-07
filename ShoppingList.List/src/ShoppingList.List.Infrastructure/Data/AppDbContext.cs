using ShoppingList.List.Core.ContributorAggregate;
using ShoppingList.List.Core.ShoppingListAggregate.Enums;

namespace ShoppingList.List.Infrastructure.Data;

public class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IDomainEventDispatcher? dispatcher
) : DbContext(options)
{
    private readonly IDomainEventDispatcher? _dispatcher = dispatcher;

    public DbSet<Contributor> Contributors => Set<Contributor>();
    public DbSet<ShoppingListEntity> ShoppingLists => Set<ShoppingListEntity>();
    public DbSet<ListItemEntity> ListItems => Set<ListItemEntity>();
    public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();
    public DbSet<ShareLinkEntity> ShareLinks => Set<ShareLinkEntity>();
    public DbSet<ListChangeEntity> ListChanges => Set<ListChangeEntity>();
    public DbSet<BatchOperationEntity> BatchOperations => Set<BatchOperationEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .HasPostgresEnum<ChangeType>(schema: null, name: "change_type")
            .HasPostgresEnum<SharePermissionType>(schema: null, name: "share_permission_type")
            .HasPostgresEnum<BatchStatus>(schema: null, name: "batch_status");

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<ShoppingListEntity>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<ListItemEntity>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<ShareLinkEntity>().HasQueryFilter(x => !x.List!.IsDeleted);
        modelBuilder.Entity<ListChangeEntity>().HasQueryFilter(x => !x.List!.IsDeleted);
        modelBuilder.Entity<BatchOperationEntity>().HasQueryFilter(x => !x.List!.IsDeleted);
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        // ignore events if no dispatcher provided
        if (_dispatcher == null)
            return result;

        // dispatch events only if save was successful
        var entitiesWithEvents = ChangeTracker
            .Entries<HasDomainEventsBase>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToArray();

        await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

        return result;
    }

    public override int SaveChanges() => SaveChangesAsync().GetAwaiter().GetResult();
}
