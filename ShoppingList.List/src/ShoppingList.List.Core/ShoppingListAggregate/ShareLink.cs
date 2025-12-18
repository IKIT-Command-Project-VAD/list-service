using ShoppingList.List.Core.ShoppingListAggregate.Enums;

namespace ShoppingList.List.Core.ShoppingListAggregate;

public sealed class ShareLink : EntityBase<Guid>, IAggregateRoot
{
    // EF Core
    private ShareLink() { }

    public Guid ListId { get; private set; }
    public ShoppingList? List { get; private set; }

    public string ShareToken { get; private set; } = default!;
    public SharePermissionType PermissionType { get; private set; }
    public Guid CreatedBy { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? ExpiresAt { get; private set; }
    public bool IsActive { get; private set; }

    public static ShareLink Create(
        Guid listId,
        Guid createdBy,
        SharePermissionType permissionType,
        DateTimeOffset? expiresAt
    )
    {
        Guard.Against.Default(listId);
        Guard.Against.Default(createdBy);

        return new ShareLink
        {
            Id = Guid.NewGuid(),
            ListId = listId,
            ShareToken = Guid.NewGuid().ToString("N")[..16],
            PermissionType = permissionType,
            CreatedBy = createdBy,
            CreatedAt = DateTimeOffset.UtcNow,
            ExpiresAt = expiresAt,
            IsActive = true,
        };
    }

    public void Update(SharePermissionType permissionType, DateTimeOffset? expiresAt, bool isActive)
    {
        PermissionType = permissionType;
        ExpiresAt = expiresAt;
        IsActive = isActive;
    }
}

