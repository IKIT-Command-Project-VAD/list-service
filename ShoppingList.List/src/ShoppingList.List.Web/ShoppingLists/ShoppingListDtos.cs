using ShoppingList.List.Core.ShoppingListAggregate.Enums;

namespace ShoppingList.List.Web.ShoppingLists;

public record ShoppingListRecord(
    Guid Id,
    Guid OwnerId,
    string Name,
    long Version,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    bool IsDeleted,
    List<ListItemRecord> Items,
    List<ShareLinkRecord> ShareLinks
);

public record ListItemRecord(
    Guid Id,
    Guid ListId,
    string Name,
    decimal Quantity,
    string? Unit,
    Guid? CategoryId,
    CategoryRecord? Category,
    decimal? Price,
    string? Currency,
    bool IsChecked,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    bool IsDeleted
);

public record CategoryRecord(Guid Id, string Name, string? Icon);

public record ShareLinkRecord(
    Guid Id,
    Guid ListId,
    string ShareToken,
    SharePermissionType PermissionType,
    Guid CreatedBy,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ExpiresAt,
    bool IsActive
);

