using Ecommerce.DAL;
using Ecommerce.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Ecommerce.Models.Home
{
    public class HomeIndexViewModel
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        dbEcommerceEntities context = new dbEcommerceEntities();
        public IEnumerable<Tbl_Product> ListOfProducts { get; set; }
        public HomeIndexViewModel CreateModel(string search)
        {
            SqlParameter[] pram = new SqlParameter[] {
                   new SqlParameter("@search",search??(object)DBNull.Value)
                   };

            IEnumerable<Tbl_Product> data = context.Database.SqlQuery<Tbl_Product>("GetBySearch @search", pram).ToList();
            return new HomeIndexViewModel
            {
                ListOfProducts = data
            };

        }
    }
}
