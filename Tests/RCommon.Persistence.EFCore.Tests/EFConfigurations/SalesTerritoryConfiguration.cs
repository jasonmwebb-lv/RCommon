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
    // SalesTerritory
    public partial class SalesTerritoryConfiguration : IEntityTypeConfiguration<SalesTerritory>
    {
        public void Configure(EntityTypeBuilder<SalesTerritory> builder)
        {
            builder.ToTable("SalesTerritory", "dbo");
            builder.HasKey(x => x.Id).HasName("PK__SalesTer__3214EC071C7A3190").IsClustered();
            builder.Ignore(x => x.AllowChangeTracking);
            //builder.Ignore(x => x.IsChanged);
            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Name).HasColumnName(@"Name").HasColumnType("nvarchar(255)").IsRequired(false).HasMaxLength(255);
            builder.Property(x => x.Description).HasColumnName(@"Description").HasColumnType("nvarchar(255)").IsRequired(false).HasMaxLength(255);

        }
    }

}
// </auto-generated>

