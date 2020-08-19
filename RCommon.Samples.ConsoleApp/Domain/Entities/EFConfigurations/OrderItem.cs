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

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RCommon.Samples.ConsoleApp.Domain.Entities
{
    // OrderItems
    public partial class OrderItem
    {
        public int OrderItemId { get; set; } // OrderItemID (Primary key)
        public decimal? Price { get; set; } // Price
        public int? Quantity { get; set; } // Quantity
        public string Store { get; set; } // Store (length: 255)
        public int? ProductId { get; set; } // ProductId
        public int? OrderId { get; set; } // OrderId

        // Foreign keys

        /// <summary>
        /// Parent Order pointed by [OrderItems].([OrderId]) (FK_Orders_OrderItems)
        /// </summary>
        public virtual Order Order { get; set; } // FK_Orders_OrderItems

        /// <summary>
        /// Parent Product pointed by [OrderItems].([ProductId]) (FK_OrderItems_Product)
        /// </summary>
        public virtual Product Product { get; set; } // FK_OrderItems_Product

        public OrderItem()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>

