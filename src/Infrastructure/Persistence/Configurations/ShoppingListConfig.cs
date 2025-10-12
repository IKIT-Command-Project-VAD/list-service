using ListService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ListService.Infrastructure.Persistence.Configurations;

public class ShoppingListConfig : IEntityTypeConfiguration<ShoppingList> 
{
    public void Configure(EntityTypeBuilder<ShoppingList> b)
    {
        b.ToTable("shopping_lists");
        b.HasKey(x => x.ListId);
        b.Property(x => x.ListId).HasColumnName("list_id").HasColumnType("uuid");
        b.Property(x => x.OwnerId).HasColumnName("owner_id").HasColumnType("uuid");
        b.Property(x => x.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
        b.Property(x => x.Version).HasColumnName("version").IsConcurrencyToken();
        b.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamptz");
        b.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        b.Property(x => x.IsDeleted).HasColumnName("is_deleted");

        b.HasMany(x => x.Items).WithOne(i => i.List).HasForeignKey(i => i.ListId);
        b.HasMany(x => x.ShareLinks).WithOne(s => s.List).HasForeignKey(s => s.ListId);
    }
}