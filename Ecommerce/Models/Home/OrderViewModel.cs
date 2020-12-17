using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce.Models.Home
{
    public class OrderViewModel
    {
        public int MemberId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Department { get; set; }
        public decimal Amount { get; set; }
        public string PaymentType { get; set; }
        public int MiPymeId { get; set; }
        public string Status { get; set; }
        public string Telephone { get; set; }
    }
}