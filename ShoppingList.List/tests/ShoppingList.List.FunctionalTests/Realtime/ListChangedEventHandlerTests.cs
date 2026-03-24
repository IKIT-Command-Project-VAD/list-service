using Microsoft.AspNetCore.SignalR;
using NSubstitute;
using ShoppingList.List.Core.ShoppingListAggregate.Events;
using ShoppingList.List.Web.Realtime;

namespace ShoppingList.List.FunctionalTests.Realtime;

public class ListChangedEventHandlerTests
{
    [Fact]
    public async Task Handle_PublishesListChangedToListGroup()
    {
        var hubContext = Substitute.For<IHubContext<ListsHub>>();
        var hubClients = Substitute.For<IHubClients>();
        var proxy = Substitute.For<IClientProxy>();
        var listId = Guid.NewGuid();

        hubContext.Clients.Returns(hubClients);
        hubClients.Group(ListsHub.GroupName(listId)).Returns(proxy);

        var handler = new ListChangedEventHandler(hubContext);
        var notification = new ListChangedEvent(listId, "list.item.updated", 42, DateTimeOffset.UtcNow);

        await handler.Handle(notification, CancellationToken.None);

        await proxy.Received(1)
            .SendCoreAsync(
                "ListChanged",
                Arg.Is<object[]>(payload =>
                    payload.Length == 1
                    && payload[0] is ListChangedMessage
                    && ((ListChangedMessage)payload[0]).ListId == listId
                    && ((ListChangedMessage)payload[0]).Version == 42
                    && ((ListChangedMessage)payload[0]).Reason == "list.item.updated"
                ),
                Arg.Any<CancellationToken>()
            );
    }
}

