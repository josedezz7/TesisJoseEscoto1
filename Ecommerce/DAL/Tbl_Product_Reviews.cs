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
    
    public partial class Tbl_Product_Reviews
    {
        public int ReviewId { get; set; }
        public Nullable<int> Rating { get; set; }
        public string Reviewer { get; set; }
        public string Comment { get; set; }
        public int ProductId { get; set; }
    
        public virtual Tbl_Product Tbl_Product { get; set; }
    }
}