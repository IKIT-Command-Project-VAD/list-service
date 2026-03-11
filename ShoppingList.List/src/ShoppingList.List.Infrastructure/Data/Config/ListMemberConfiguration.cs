using ShoppingList.List.Core.ShoppingListAggregate;

namespace ShoppingList.List.Infrastructure.Data.Config;

public class ListMemberConfiguration : IEntityTypeConfiguration<ListMember>
{
    public void Configure(EntityTypeBuilder<ListMember> builder)
    {
        builder.ToTable("list_members");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("member_id").HasColumnType("uuid");
        builder.Property(x => x.ListId).HasColumnName("list_id").HasColumnType("uuid");
        builder.Property(x => x.UserId).HasColumnName("user_id").HasColumnType("uuid");
        builder.Property(x => x.PermissionType).HasColumnName("permission_type");
        builder.Property(x => x.JoinedAt).HasColumnName("joined_at").HasColumnType("timestamptz");

        builder.HasOne(x => x.List).WithMany(l => l.Members).HasForeignKey(x => x.ListId);
    }
}
