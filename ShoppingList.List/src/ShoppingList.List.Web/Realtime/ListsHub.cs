using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Ardalis.SharedKernel;
using ShoppingList.List.Core.ShoppingListAggregate.Specifications;

namespace ShoppingList.List.Web.Realtime;

[Authorize]
public sealed class ListsHub(IReadRepository<ShoppingListEntity> listRepository) : Hub
{
    public static string GroupName(Guid listId) => $"list:{listId:D}";

    public async Task JoinListGroup(Guid listId)
    {
        var userId = Context.User?.GetUserIdAsGuid();
        if (userId is null)
        {
            throw new HubException("Unauthorized");
        }

        var list = await listRepository.FirstOrDefaultAsync(
            new ShoppingListByIdWithDetailsSpec(listId, userId.Value),
            Context.ConnectionAborted
        );

        if (list is null)
        {
            throw new HubException("Forbidden");
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, GroupName(listId), Context.ConnectionAborted);
    }

    public async Task LeaveListGroup(Guid listId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupName(listId), Context.ConnectionAborted);
    }
}

