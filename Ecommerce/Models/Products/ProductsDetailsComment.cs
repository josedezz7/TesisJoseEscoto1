using Ecommerce.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce.Models.Products
{
    public class ProductsDetailsComment
    {
        public Tbl_Product product { get; set; }
        public Tbl_Product_Reviews review { get; set; }
        public List<Tbl_Product_Reviews> reviews { get; set; }
    }
}