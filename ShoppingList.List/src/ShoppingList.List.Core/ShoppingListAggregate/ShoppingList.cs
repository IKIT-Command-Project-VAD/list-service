using ShoppingList.List.Core.ShoppingListAggregate.Enums;

namespace ShoppingList.List.Core.ShoppingListAggregate;

public sealed class ShoppingList : EntityBase<Guid>, IAggregateRoot
{
    private readonly List<ListItem> _items = new();
    private readonly List<ShareLink> _shareLinks = new();

    public Guid OwnerId { get; private set; }
    public string Name { get; private set; } = default!;
    public long Version { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public bool IsDeleted { get; private set; }

    public IReadOnlyCollection<ListItem> Items => _items.AsReadOnly();
    public IReadOnlyCollection<ShareLink> ShareLinks => _shareLinks.AsReadOnly();

    // EF Core
    private ShoppingList() { }

    public static ShoppingList Create(Guid ownerId, string name)
    {
        return new ShoppingList
        {
            Id = Guid.NewGuid(),
            OwnerId = Guard.Against.Default(ownerId),
            Name = Guard.Against.NullOrWhiteSpace(name),
            Version = 1,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            IsDeleted = false,
        };
    }

    public void UpdateName(string name)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
        Touch();
    }

    public void SoftDelete()
    {
        if (IsDeleted)
            return;

        IsDeleted = true;
        Touch();
    }

    public ListItem AddItem(
        string name,
        decimal quantity,
        string? unit,
        Guid? categoryId,
        decimal? price,
        string? currency,
        bool isChecked,
        Guid? itemId = null
    )
    {
        var item = ListItem.Create(
            listId: Id,
            name: name,
            quantity: quantity,
            unit: unit,
            categoryId: categoryId,
            price: price,
            currency: currency,
            isChecked: isChecked,
            itemId: itemId ?? Guid.NewGuid()
        );

        _items.Add(item);
        Touch();
        return item;
    }

    public ShareLink AddShareLink(
        Guid createdBy,
        SharePermissionType permissionType,
        DateTimeOffset? expiresAt
    )
    {
        var link = ShareLink.Create(Id, createdBy, permissionType, expiresAt);
        _shareLinks.Add(link);
        Touch();
        return link;
    }

    internal void Touch()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
        Version++;
    }
}

