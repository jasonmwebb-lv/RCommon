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

namespace RCommon.Persistence.EFCore.Tests
{
    // SalesPerson
    public partial class SalesPersonConfiguration : IEntityTypeConfiguration<SalesPerson>
    {
        public void Configure(EntityTypeBuilder<SalesPerson> builder)
        {
            builder.ToTable("SalesPerson", "dbo");
            builder.HasKey(x => x.Id).HasName("PK__SalesPer__3214EC0722B792EE").IsClustered();
            builder.Ignore(x => x.AllowChangeTracking);
            //builder.Ignore(x => x.IsChanged);
            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.FirstName).HasColumnName(@"FirstName").HasColumnType("nvarchar(255)").IsRequired(false).HasMaxLength(255);
            builder.Property(x => x.LastName).HasColumnName(@"LastName").HasColumnType("nvarchar(255)").IsRequired(false).HasMaxLength(255);
            builder.Property(x => x.SalesQuota).HasColumnName(@"SalesQuota").HasColumnType("real").IsRequired(false);
            builder.Property(x => x.SalesYtd).HasColumnName(@"SalesYTD").HasColumnType("decimal(19,5)").IsRequired(false);
            builder.Property(x => x.DepartmentId).HasColumnName(@"DepartmentId").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.TerritoryId).HasColumnName(@"TerritoryId").HasColumnType("int").IsRequired(false);

            // Foreign keys
            builder.HasOne(a => a.Department).WithMany(b => b.SalesPersons).HasForeignKey(c => c.DepartmentId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK74214A90E25FF6");
            builder.HasOne(a => a.SalesTerritory).WithMany(b => b.SalesPersons).HasForeignKey(c => c.TerritoryId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK74214A90B23DB0A3");

        }
    }

}
// </auto-generated>

