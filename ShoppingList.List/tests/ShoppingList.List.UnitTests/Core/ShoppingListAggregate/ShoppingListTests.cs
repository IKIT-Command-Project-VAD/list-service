using ShoppingList.List.Core.ShoppingListAggregate.Enums;
using SL = ShoppingList.List.Core.ShoppingListAggregate.ShoppingList;

namespace ShoppingList.List.UnitTests.Core.ShoppingListAggregate;

public class ShoppingListTests
{
    [Fact]
    public void Create_ValidParameters_CreatesShoppingList()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var name = "Weekly Groceries";

        // Act
        var shoppingList = SL.Create(ownerId, name);

        // Assert
        Assert.NotEqual(Guid.Empty, shoppingList.Id);
        Assert.Equal(ownerId, shoppingList.OwnerId);
        Assert.Equal(name, shoppingList.Name);
        Assert.Equal(1, shoppingList.Version);
        Assert.False(shoppingList.IsDeleted);
        Assert.True(shoppingList.CreatedAt <= DateTimeOffset.UtcNow);
        Assert.Equal(shoppingList.CreatedAt, shoppingList.UpdatedAt);
        Assert.Empty(shoppingList.Items);
        Assert.Empty(shoppingList.ShareLinks);
    }

    [Fact]
    public void Create_DefaultOwnerId_ThrowsArgumentException()
    {
        // Arrange
        var defaultOwnerId = Guid.Empty;
        var name = "Valid Name";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => SL.Create(defaultOwnerId, name));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_InvalidName_ThrowsArgumentException(string invalidName)
    {
        // Arrange
        var ownerId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => SL.Create(ownerId, invalidName));
    }

    [Fact]
    public void UpdateName_ValidName_UpdatesNameAndTouches()
    {
        // Arrange
        var shoppingList = SL.Create(Guid.NewGuid(), "Old Name");
        var newName = "New Name";
        var originalUpdatedAt = shoppingList.UpdatedAt;
        var originalVersion = shoppingList.Version;

        // Act
        shoppingList.UpdateName(newName);

        // Assert
        Assert.Equal(newName, shoppingList.Name);
        Assert.True(shoppingList.UpdatedAt > originalUpdatedAt);
        Assert.Equal(originalVersion + 1, shoppingList.Version);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateName_InvalidName_ThrowsArgumentException(string invalidName)
    {
        // Arrange
        var shoppingList = SL.Create(Guid.NewGuid(), "Valid Name");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => shoppingList.UpdateName(invalidName));
    }

    [Fact]
    public void SoftDelete_NotDeleted_SetsIsDeletedAndTouches()
    {
        // Arrange
        var shoppingList = SL.Create(Guid.NewGuid(), "Name");
        var originalUpdatedAt = shoppingList.UpdatedAt;
        var originalVersion = shoppingList.Version;

        // Act
        shoppingList.SoftDelete();

        // Assert
        Assert.True(shoppingList.IsDeleted);
        Assert.True(shoppingList.UpdatedAt > originalUpdatedAt);
        Assert.Equal(originalVersion + 1, shoppingList.Version);
    }

    [Fact]
    public void SoftDelete_AlreadyDeleted_DoesNothing()
    {
        // Arrange
        var shoppingList = SL.Create(Guid.NewGuid(), "Name");
        shoppingList.SoftDelete(); // First delete
        var updatedAtAfterFirstDelete = shoppingList.UpdatedAt;
        var versionAfterFirstDelete = shoppingList.Version;

        // Act
        shoppingList.SoftDelete(); // Second delete

        // Assert
        Assert.True(shoppingList.IsDeleted);
        Assert.Equal(updatedAtAfterFirstDelete, shoppingList.UpdatedAt);
        Assert.Equal(versionAfterFirstDelete, shoppingList.Version);
    }

    [Fact]
    public void AddItem_ValidParameters_AddsItemAndTouches()
    {
        // Arrange
        var shoppingList = SL.Create(Guid.NewGuid(), "Name");
        var name = "Milk";
        var quantity = 2.5m;
        var unit = "liters";
        var categoryId = Guid.NewGuid();
        var price = 3.99m;
        var currency = "USD";
        var isChecked = false;
        var originalUpdatedAt = shoppingList.UpdatedAt;
        var originalVersion = shoppingList.Version;

        // Act
        var addedItem = shoppingList.AddItem(
            name,
            quantity,
            unit,
            categoryId,
            price,
            currency,
            isChecked
        );

        // Assert
        Assert.Single(shoppingList.Items);
        Assert.Equal(addedItem, shoppingList.Items.First());
        Assert.True(shoppingList.UpdatedAt > originalUpdatedAt);
        Assert.Equal(originalVersion + 1, shoppingList.Version);
    }

    [Fact]
    public void AddItem_WithCustomItemId_AddsItemWithSpecifiedId()
    {
        // Arrange
        var shoppingList = SL.Create(Guid.NewGuid(), "Name");
        var customItemId = Guid.NewGuid();

        // Act
        var addedItem = shoppingList.AddItem(
            "Milk",
            1m,
            null,
            null,
            null,
            null,
            false,
            customItemId
        );

        // Assert
        Assert.Equal(customItemId, addedItem.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void AddItem_InvalidName_ThrowsArgumentException(string invalidName)
    {
        // Arrange
        var shoppingList = SL.Create(Guid.NewGuid(), "Name");

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            shoppingList.AddItem(invalidName, 1m, null, null, null, null, false)
        );
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-0.1)]
    public void AddItem_NegativeQuantity_ThrowsArgumentException(decimal negativeQuantity)
    {
        // Arrange
        var shoppingList = SL.Create(Guid.NewGuid(), "Name");

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            shoppingList.AddItem("Valid Name", negativeQuantity, null, null, null, null, false)
        );
    }

    [Fact]
    public void AddShareLink_ValidParameters_AddsShareLinkAndTouches()
    {
        // Arrange
        var shoppingList = SL.Create(Guid.NewGuid(), "Name");
        var createdBy = Guid.NewGuid();
        var permissionType = SharePermissionType.Read;
        var expiresAt = DateTimeOffset.UtcNow.AddDays(7);
        var originalUpdatedAt = shoppingList.UpdatedAt;
        var originalVersion = shoppingList.Version;

        // Act
        var addedLink = shoppingList.AddShareLink(createdBy, permissionType, expiresAt);

        // Assert
        Assert.Single(shoppingList.ShareLinks);
        Assert.Equal(addedLink, shoppingList.ShareLinks.First());
        Assert.True(shoppingList.UpdatedAt > originalUpdatedAt);
        Assert.Equal(originalVersion + 1, shoppingList.Version);
    }

    [Fact]
    public void AddShareLink_NullExpiresAt_AddsIndefiniteShareLink()
    {
        // Arrange
        var shoppingList = SL.Create(Guid.NewGuid(), "Name");
        var createdBy = Guid.NewGuid();
        var permissionType = SharePermissionType.Write;

        // Act
        var addedLink = shoppingList.AddShareLink(createdBy, permissionType, null);

        // Assert
        Assert.Null(addedLink.ExpiresAt);
    }
}
