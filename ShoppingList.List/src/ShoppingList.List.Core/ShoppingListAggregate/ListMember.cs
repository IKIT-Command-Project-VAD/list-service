using ShoppingList.List.Core.ShoppingListAggregate.Enums;

namespace ShoppingList.List.Core.ShoppingListAggregate;

public sealed class ListMember : EntityBase<Guid>, IAggregateRoot
{
    // EF Core
    private ListMember() { }

    public Guid ListId { get; private set; }
    public ShoppingList? List { get; private set; }

    public Guid UserId { get; private set; }
    public SharePermissionType PermissionType { get; private set; }

    public DateTimeOffset JoinedAt { get; private set; }

    public static ListMember Create(
        Guid listId,
        Guid userId,
        SharePermissionType permissionType
    )
    {
        Guard.Against.Default(listId);
        Guard.Against.Default(userId);

        return new ListMember
        {
            Id = Guid.NewGuid(),
            ListId = listId,
            UserId = userId,
            PermissionType = permissionType,
            JoinedAt = DateTimeOffset.UtcNow,
        };
    }
}
