namespace ShoppingList.List.Web.Realtime;

public sealed record ListChangedMessage(
    Guid ListId,
    long? Version,
    string Reason,
    DateTimeOffset ChangedAt
);

