using System.Security.Claims;
using Ardalis.SharedKernel;
using Ardalis.Specification;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;
using NSubstitute;
using ShoppingListAggregate = ShoppingList.List.Core.ShoppingListAggregate.ShoppingList;
using ShoppingList.List.Core.ShoppingListAggregate.Specifications;
using ShoppingList.List.Web.Realtime;

namespace ShoppingList.List.FunctionalTests.Realtime;

public class ListsHubTests
{
    [Fact]
    public async Task JoinListGroup_UnauthorizedUser_ThrowsHubException()
    {
        var repository = Substitute.For<IReadRepository<ShoppingListAggregate>>();
        var hub = new ListsHub(repository)
        {
            Context = new TestHubCallerContext(new ClaimsPrincipal(new ClaimsIdentity())),
            Groups = new RecordingGroupManager(),
        };

        var ex = await Assert.ThrowsAsync<HubException>(() => hub.JoinListGroup(Guid.NewGuid()));

        ex.Message.ShouldBe("Unauthorized");
    }

    [Fact]
    public async Task JoinListGroup_UserWithoutAccess_ThrowsHubException()
    {
        var repository = Substitute.For<IReadRepository<ShoppingListAggregate>>();
        repository
            .FirstOrDefaultAsync(Arg.Any<ISpecification<ShoppingListAggregate>>(), Arg.Any<CancellationToken>())
            .Returns((ShoppingListAggregate?)null);

        var userId = Guid.NewGuid();
        var identity = new ClaimsIdentity(
            new[] { new Claim("sub", userId.ToString()) },
            authenticationType: "Test"
        );

        var hub = new ListsHub(repository)
        {
            Context = new TestHubCallerContext(new ClaimsPrincipal(identity)),
            Groups = new RecordingGroupManager(),
        };

        var ex = await Assert.ThrowsAsync<HubException>(() => hub.JoinListGroup(Guid.NewGuid()));

        ex.Message.ShouldBe("Forbidden");
    }

    [Fact]
    public async Task JoinListGroup_UserWithAccess_AddsConnectionToExpectedGroup()
    {
        var listId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var repository = Substitute.For<IReadRepository<ShoppingListAggregate>>();
        var list = ShoppingListAggregate.Create(userId, "Shared list");

        repository
            .FirstOrDefaultAsync(Arg.Any<ShoppingListByIdWithDetailsSpec>(), Arg.Any<CancellationToken>())
            .Returns(list);

        var identity = new ClaimsIdentity(
            new[] { new Claim("sub", userId.ToString()) },
            authenticationType: "Test"
        );

        var groups = new RecordingGroupManager();
        var hub = new ListsHub(repository)
        {
            Context = new TestHubCallerContext(new ClaimsPrincipal(identity)),
            Groups = groups,
        };

        await hub.JoinListGroup(listId);

        groups.AddedGroups.ShouldContain((hub.Context.ConnectionId, ListsHub.GroupName(listId)));
    }

    private sealed class RecordingGroupManager : IGroupManager
    {
        public List<(string ConnectionId, string GroupName)> AddedGroups { get; } = new();

        public Task AddToGroupAsync(
            string connectionId,
            string groupName,
            CancellationToken cancellationToken = default
        )
        {
            AddedGroups.Add((connectionId, groupName));
            return Task.CompletedTask;
        }

        public Task RemoveFromGroupAsync(
            string connectionId,
            string groupName,
            CancellationToken cancellationToken = default
        ) => Task.CompletedTask;
    }

    private sealed class TestHubCallerContext : HubCallerContext
    {
        private readonly ClaimsPrincipal _user;
        private readonly IDictionary<object, object?> _items = new Dictionary<object, object?>();

        public TestHubCallerContext(ClaimsPrincipal user)
        {
            _user = user;
        }

        public override string ConnectionId { get; } = Guid.NewGuid().ToString("N");
        public override string? UserIdentifier => _user.FindFirstValue(ClaimTypes.NameIdentifier);
        public override ClaimsPrincipal? User => _user;
        public override IDictionary<object, object?> Items => _items;
        public override IFeatureCollection Features { get; } = new FeatureCollection();
        public override CancellationToken ConnectionAborted { get; } = CancellationToken.None;
        public override void Abort() { }
    }
}

