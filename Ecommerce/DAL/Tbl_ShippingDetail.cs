//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ecommerce.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_ShippingDetail
    {
        public int ShippingDetailId { get; set; }
        public int ShippingId { get; set; }
        public int ProductId { get; set; }
        public Nullable<int> Quantity { get; set; }
    
        public virtual Tbl_Shipping Tbl_Shipping { get; set; }
        public virtual Tbl_Product Tbl_Product { get; set; }
    }
}
