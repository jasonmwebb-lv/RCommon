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



using RCommon.BusinessEntities;
using System;


using System.Collections.Generic;


using System.ComponentModel.DataAnnotations;


using System.Threading;


using System.Threading.Tasks;


namespace Samples.Domain.Entities
{
    




// DiveLocationDetails
    


public partial class DiveLocationDetail : BusinessEntity
    
{
    





    /// <summary>
    
    /// DiveLocationId (Primary key)
    
    /// </summary>
    


    [Key]
    

    [Required]
    

    [Display(Name = "Dive location ID")]
    

    public Guid DiveLocationId { get; set; }

    

    /// <summary>
    
    /// ImageData
    
    /// </summary>
    


    [Display(Name = "Image data")]
    

    public byte[] ImageData { get; set; }

    
    // Reverse navigation

    
    /// <summary>
    
    /// Parent (One-to-One) DiveLocationDetail pointed by [DiveLocations].[Id] (FK_DiveLocations_DiveLocationDetails)
    
    /// </summary>
    






    public virtual DiveLocation DiveLocation { get; set; } // DiveLocations.FK_DiveLocations_DiveLocationDetails

    
    public DiveLocationDetail()
    
    {
    






        InitializePartial();
    


    }

    
    partial void InitializePartial();

        public override object[] GetKeys()
        {
            return new object[] { this.DiveLocationId };
        }
    }

}
// </auto-generated>

