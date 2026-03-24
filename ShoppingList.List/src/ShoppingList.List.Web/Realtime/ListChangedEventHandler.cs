using Microsoft.AspNetCore.SignalR;
using ShoppingList.List.Core.ShoppingListAggregate.Events;

namespace ShoppingList.List.Web.Realtime;

public sealed class ListChangedEventHandler(IHubContext<ListsHub> hubContext)
    : INotificationHandler<ListChangedEvent>
{
    public async Task Handle(ListChangedEvent notification, CancellationToken cancellationToken)
    {
        var payload = new ListChangedMessage(
            notification.ListId,
            notification.Version,
            notification.Reason,
            notification.ChangedAt
        );

        await hubContext.Clients
            .Group(ListsHub.GroupName(notification.ListId))
            .SendAsync("ListChanged", payload, cancellationToken);
    }
}

