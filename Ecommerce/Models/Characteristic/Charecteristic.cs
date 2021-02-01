using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce.Models.Characteristic
{
    public class Charecteristic
    {
        public int productId { get; set; }

        public IEnumerable<Ecommerce.DAL.Tbl_Product_Characteristics> Characteristics { get; set; }
    }
}