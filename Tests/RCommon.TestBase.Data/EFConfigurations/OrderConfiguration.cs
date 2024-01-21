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
using RCommon.TestBase.Entities;

namespace RCommon.TestBase.Data
{
    // Orders
    public partial class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders", "dbo");
            builder.HasKey(x => x.Id).HasName("PK__Orders__C3905BAF964CE0E8").IsClustered();
            builder.Ignore(x => x.AllowEventTracking);
            //builder.Ignore(x => x.IsChanged);
            builder.Property(x => x.Id).HasColumnName(@"OrderID").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.OrderDate).HasColumnName(@"OrderDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.ShipDate).HasColumnName(@"ShipDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.CustomerId).HasColumnName(@"CustomerId").HasColumnType("int").IsRequired(false);

            // Foreign keys
            builder.HasOne(a => a.Customer).WithMany(b => b.Orders).HasForeignKey(c => c.CustomerId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Customer_Orders");

        }
    }

}
// </auto-generated>
