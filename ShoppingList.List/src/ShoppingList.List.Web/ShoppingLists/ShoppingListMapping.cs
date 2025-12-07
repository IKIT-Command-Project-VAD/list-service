namespace ShoppingList.List.Web.ShoppingLists;

internal static class ShoppingListMapping
{
    public static ShoppingListRecord ToRecord(this ShoppingListEntity list) =>
        new(
            list.Id,
            list.OwnerId,
            list.Name,
            list.Version,
            list.CreatedAt,
            list.UpdatedAt,
            list.IsDeleted,
            list.Items.Select(i => i.ToRecord()).ToList(),
            list.ShareLinks.Select(s => s.ToRecord()).ToList()
        );

    public static ListItemRecord ToRecord(this ListItemEntity item) =>
        new(
            item.Id,
            item.ListId,
            item.Name,
            item.Quantity,
            item.Unit,
            item.CategoryId,
            item.Category?.ToRecord(),
            item.Price,
            item.Currency,
            item.IsChecked,
            item.CreatedAt,
            item.UpdatedAt,
            item.IsDeleted
        );

    public static CategoryRecord ToRecord(this CategoryEntity category) =>
        new(category.Id, category.Name, category.Icon);

    public static ShareLinkRecord ToRecord(this ShareLinkEntity link) =>
        new(
            link.Id,
            link.ListId,
            link.ShareToken,
            link.PermissionType,
            link.CreatedBy,
            link.CreatedAt,
            link.ExpiresAt,
            link.IsActive
        );
}

