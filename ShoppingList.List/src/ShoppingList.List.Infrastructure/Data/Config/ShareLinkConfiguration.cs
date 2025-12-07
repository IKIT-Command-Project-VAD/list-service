using ShoppingList.List.Core.ShoppingListAggregate;
using ShoppingList.List.Core.ShoppingListAggregate.Enums;

namespace ShoppingList.List.Infrastructure.Data.Config;

public class ShareLinkConfiguration : IEntityTypeConfiguration<ShareLink>
{
    public void Configure(EntityTypeBuilder<ShareLink> builder)
    {
        builder.ToTable("share_links");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("share_id").HasColumnType("uuid");
        builder.Property(x => x.ListId).HasColumnName("list_id").HasColumnType("uuid");
        builder.Property(x => x.ShareToken).HasColumnName("share_token").HasMaxLength(255).IsRequired();
        builder
            .Property(x => x.PermissionType)
            .HasColumnName("share_permission_type")
            .HasColumnType("share_permission_type");
        builder.Property(x => x.CreatedBy).HasColumnName("created_by").HasColumnType("uuid");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamptz");
        builder.Property(x => x.ExpiresAt).HasColumnName("expires_at").HasColumnType("timestamptz");
        builder.Property(x => x.IsActive).HasColumnName("is_active");
    }
}

