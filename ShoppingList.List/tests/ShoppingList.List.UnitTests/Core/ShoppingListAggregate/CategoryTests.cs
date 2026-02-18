using ShoppingList.List.Core.ShoppingListAggregate;

namespace ShoppingList.List.UnitTests.Core.ShoppingListAggregate;

public class CategoryTests
{
    [Fact]
    public void Constructor_ValidNameAndNullIcon_CreatesCategory()
    {
        // Arrange
        var name = "Groceries";

        // Act
        var category = new Category(name);

        // Assert
        Assert.NotEqual(Guid.Empty, category.Id);
        Assert.Equal(name, category.Name);
        Assert.Null(category.Icon);
    }

    [Fact]
    public void Constructor_ValidNameAndIcon_CreatesCategory()
    {
        // Arrange
        var name = "Groceries";
        var icon = "icon.png";

        // Act
        var category = new Category(name, icon);

        // Assert
        Assert.NotEqual(Guid.Empty, category.Id);
        Assert.Equal(name, category.Name);
        Assert.Equal(icon, category.Icon);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_InvalidName_ThrowsArgumentException(string invalidName)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Category(invalidName));
    }

    [Fact]
    public void Update_ValidNameAndNullIcon_UpdatesCategory()
    {
        // Arrange
        var category = new Category("Old Name", "old-icon.png");
        var newName = "New Name";

        // Act
        category.Update(newName, null);

        // Assert
        Assert.Equal(newName, category.Name);
        Assert.Null(category.Icon);
    }

    [Fact]
    public void Update_ValidNameAndIcon_UpdatesCategory()
    {
        // Arrange
        var category = new Category("Old Name");
        var newName = "New Name";
        var newIcon = "new-icon.png";

        // Act
        category.Update(newName, newIcon);

        // Assert
        Assert.Equal(newName, category.Name);
        Assert.Equal(newIcon, category.Icon);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Update_InvalidName_ThrowsArgumentException(string invalidName)
    {
        // Arrange
        var category = new Category("Valid Name");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => category.Update(invalidName, null));
    }
}
