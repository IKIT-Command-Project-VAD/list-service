using ListService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ListService.Infrastructure.Persistence.Configurations;

public class ShareLinkConfig : IEntityTypeConfiguration<ShareLink>
{
    public void Configure(EntityTypeBuilder<ShareLink> b)
    {
        b.ToTable("share_links");
        b.HasKey(x => x.ShareId);
        b.Property(x => x.ShareId).HasColumnName("share_id").HasColumnType("uuid");
        b.Property(x => x.ListId).HasColumnName("list_id").HasColumnType("uuid");
        b.Property(x => x.ShareToken).HasColumnName("share_token").HasMaxLength(255).IsRequired();
        b.Property(x => x.PermissionType).HasColumnName("permission_type").HasColumnName("share_permission_type");
        b.Property(x => x.CreatedBy).HasColumnName("created_by").HasColumnType("uuid");
        b.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamptz");
        b.Property(x => x.ExpiresAt).HasColumnName("expires_at").HasColumnType("timestamptz");
        b.Property(x => x.IsActive).HasColumnName("is_active");
    }
}