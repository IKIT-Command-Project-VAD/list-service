using ShoppingList.List.Core.ShoppingListAggregate;

namespace ShoppingList.List.UnitTests.Core.ShoppingListAggregate;

public class ListItemTests
{
    [Fact]
    public void Create_ValidParameters_CreatesListItem()
    {
        // Arrange
        var listId = Guid.NewGuid();
        var name = "Milk";
        var quantity = 2.5m;
        var unit = "liters";
        var categoryId = Guid.NewGuid();
        var price = 3.99m;
        var currency = "USD";
        var isChecked = false;
        var itemId = Guid.NewGuid();

        // Act
        var listItem = ListItem.Create(
            listId,
            name,
            quantity,
            unit,
            categoryId,
            price,
            currency,
            isChecked,
            itemId
        );

        // Assert
        Assert.Equal(itemId, listItem.Id);
        Assert.Equal(listId, listItem.ListId);
        Assert.Equal(name, listItem.Name);
        Assert.Equal(quantity, listItem.Quantity);
        Assert.Equal(unit, listItem.Unit);
        Assert.Equal(categoryId, listItem.CategoryId);
        Assert.Equal(price, listItem.Price);
        Assert.Equal(currency, listItem.Currency);
        Assert.Equal(isChecked, listItem.IsChecked);
        Assert.False(listItem.IsDeleted);
        Assert.True(listItem.CreatedAt <= DateTimeOffset.UtcNow);
        Assert.Equal(listItem.CreatedAt, listItem.UpdatedAt);
    }

    [Fact]
    public void Create_DefaultListId_ThrowsArgumentException()
    {
        // Arrange
        var defaultListId = Guid.Empty;
        var name = "Milk";
        var quantity = 2.5m;
        var itemId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            ListItem.Create(defaultListId, name, quantity, null, null, null, null, false, itemId)
        );
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_InvalidName_ThrowsArgumentException(string invalidName)
    {
        // Arrange
        var listId = Guid.NewGuid();
        var quantity = 2.5m;
        var itemId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            ListItem.Create(listId, invalidName, quantity, null, null, null, null, false, itemId)
        );
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-0.1)]
    public void Create_NegativeQuantity_ThrowsArgumentException(decimal negativeQuantity)
    {
        // Arrange
        var listId = Guid.NewGuid();
        var name = "Milk";
        var itemId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            ListItem.Create(listId, name, negativeQuantity, null, null, null, null, false, itemId)
        );
    }

    [Fact]
    public void Create_ZeroQuantity_CreatesListItem()
    {
        // Arrange
        var listId = Guid.NewGuid();
        var name = "Milk";
        var quantity = 0m;
        var itemId = Guid.NewGuid();

        // Act
        var listItem = ListItem.Create(
            listId,
            name,
            quantity,
            null,
            null,
            null,
            null,
            false,
            itemId
        );

        // Assert
        Assert.Equal(quantity, listItem.Quantity);
    }

    [Fact]
    public void Update_ValidParameters_UpdatesListItem()
    {
        // Arrange
        var listId = Guid.NewGuid();
        var originalItem = ListItem.Create(
            listId,
            "Old Name",
            1m,
            "old unit",
            null,
            null,
            null,
            false,
            Guid.NewGuid()
        );
        var newName = "New Name";
        var newQuantity = 2.5m;
        var newUnit = "new unit";
        var newCategoryId = Guid.NewGuid();
        var newPrice = 4.99m;
        var newCurrency = "EUR";
        var newIsChecked = true;
        var originalUpdatedAt = originalItem.UpdatedAt;

        // Act
        originalItem.Update(
            newName,
            newQuantity,
            newUnit,
            newCategoryId,
            newPrice,
            newCurrency,
            newIsChecked
        );

        // Assert
        Assert.Equal(newName, originalItem.Name);
        Assert.Equal(newQuantity, originalItem.Quantity);
        Assert.Equal(newUnit, originalItem.Unit);
        Assert.Equal(newCategoryId, originalItem.CategoryId);
        Assert.Equal(newPrice, originalItem.Price);
        Assert.Equal(newCurrency, originalItem.Currency);
        Assert.Equal(newIsChecked, originalItem.IsChecked);
        Assert.True(originalItem.UpdatedAt > originalUpdatedAt);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Update_InvalidName_ThrowsArgumentException(string invalidName)
    {
        // Arrange
        var listItem = ListItem.Create(
            Guid.NewGuid(),
            "Valid Name",
            1m,
            null,
            null,
            null,
            null,
            false,
            Guid.NewGuid()
        );

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            listItem.Update(invalidName, 1m, null, null, null, null, false)
        );
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-0.1)]
    public void Update_NegativeQuantity_ThrowsArgumentArgumentException(decimal negativeQuantity)
    {
        // Arrange
        var listItem = ListItem.Create(
            Guid.NewGuid(),
            "Valid Name",
            1m,
            null,
            null,
            null,
            null,
            false,
            Guid.NewGuid()
        );

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            listItem.Update("Valid Name", negativeQuantity, null, null, null, null, false)
        );
    }

    [Fact]
    public void ToggleChecked_NoParameter_FlipsIsChecked()
    {
        // Arrange
        var listItem = ListItem.Create(
            Guid.NewGuid(),
            "Name",
            1m,
            null,
            null,
            null,
            null,
            false,
            Guid.NewGuid()
        );
        var originalUpdatedAt = listItem.UpdatedAt;

        // Act
        listItem.ToggleChecked();

        // Assert
        Assert.True(listItem.IsChecked);
        Assert.True(listItem.UpdatedAt > originalUpdatedAt);
    }

    [Fact]
    public void ToggleChecked_WithTrue_SetsIsCheckedToTrue()
    {
        // Arrange
        var listItem = ListItem.Create(
            Guid.NewGuid(),
            "Name",
            1m,
            null,
            null,
            null,
            null,
            false,
            Guid.NewGuid()
        );
        var originalUpdatedAt = listItem.UpdatedAt;

        // Act
        listItem.ToggleChecked(true);

        // Assert
        Assert.True(listItem.IsChecked);
        Assert.True(listItem.UpdatedAt > originalUpdatedAt);
    }

    [Fact]
    public void ToggleChecked_WithFalse_SetsIsCheckedToFalse()
    {
        // Arrange
        var listItem = ListItem.Create(
            Guid.NewGuid(),
            "Name",
            1m,
            null,
            null,
            null,
            null,
            true,
            Guid.NewGuid()
        );
        var originalUpdatedAt = listItem.UpdatedAt;

        // Act
        listItem.ToggleChecked(false);

        // Assert
        Assert.False(listItem.IsChecked);
        Assert.True(listItem.UpdatedAt > originalUpdatedAt);
    }

    [Fact]
    public void SoftDelete_NotDeleted_SetsIsDeletedAndUpdatesTimestamp()
    {
        // Arrange
        var listItem = ListItem.Create(
            Guid.NewGuid(),
            "Name",
            1m,
            null,
            null,
            null,
            null,
            false,
            Guid.NewGuid()
        );
        var originalUpdatedAt = listItem.UpdatedAt;

        // Act
        listItem.SoftDelete();

        // Assert
        Assert.True(listItem.IsDeleted);
        Assert.True(listItem.UpdatedAt > originalUpdatedAt);
    }

    [Fact]
    public void SoftDelete_AlreadyDeleted_DoesNothing()
    {
        // Arrange
        var listItem = ListItem.Create(
            Guid.NewGuid(),
            "Name",
            1m,
            null,
            null,
            null,
            null,
            false,
            Guid.NewGuid()
        );
        listItem.SoftDelete(); // First delete
        var updatedAtAfterFirstDelete = listItem.UpdatedAt;

        // Act
        listItem.SoftDelete(); // Second delete

        // Assert
        Assert.True(listItem.IsDeleted);
        Assert.Equal(updatedAtAfterFirstDelete, listItem.UpdatedAt); // No change
    }
}
