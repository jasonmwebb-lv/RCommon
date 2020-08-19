// ------------------------------------------------------------------------------------------------

// <auto-generated>
// ReSharper disable CheckNamespace
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable EmptyNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable NotAccessedVariable
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantCast
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// ReSharper disable UsePatternMatching
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RCommon.Samples.ConsoleApp.Domain.Entities
{
    // OrderItems
    public partial class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems", "dbo");
            builder.HasKey(x => x.OrderItemId).HasName("PK__OrderIte__57ED06A1FD2C18E0").IsClustered();

            builder.Property(x => x.OrderItemId).HasColumnName(@"OrderItemID").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Price).HasColumnName(@"Price").HasColumnType("decimal(19,5)").IsRequired(false);
            builder.Property(x => x.Quantity).HasColumnName(@"Quantity").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.Store).HasColumnName(@"Store").HasColumnType("nvarchar(255)").IsRequired(false).HasMaxLength(255);
            builder.Property(x => x.ProductId).HasColumnName(@"ProductId").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.OrderId).HasColumnName(@"OrderId").HasColumnType("int").IsRequired(false);

            // Foreign keys
            builder.HasOne(a => a.Order).WithMany(b => b.OrderItems).HasForeignKey(c => c.OrderId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Orders_OrderItems");
            builder.HasOne(a => a.Product).WithMany(b => b.OrderItems).HasForeignKey(c => c.ProductId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_OrderItems_Product");

        }
    }

}
// </auto-generated>

