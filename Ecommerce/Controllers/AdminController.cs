using Ecommerce.DAL;
using Ecommerce.Models;
using Ecommerce.Repository;
using Newtonsoft.Json;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Controllers
{
    //[Authorize(Roles = "MiPymes")]
    public class AdminController : Controller
    {
        // GET: Admin
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        dbEcommerceEntities1 ctx = new dbEcommerceEntities1();

        public ActionResult Index()
        {
           return RedirectToAction("Dashboard");
        }

            public List<SelectListItem> GetCategory()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var cat = _unitOfWork.GetRepositoryInstance<Tbl_Category>().GetAllRecord();
            foreach (var item in cat)
            {
                list.Add(new SelectListItem { Value = item.CategoryId.ToString(), Text = item.CategoryName });
            }
            return list;

        }
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Categories()
        {
            List<Tbl_Category> allcategories = _unitOfWork.GetRepositoryInstance<Tbl_Category>().GetAllRecordsIQueryable().Where(i => i.IsDelete == false).ToList();
            return View(allcategories);
        }

        public ActionResult AddCategory()
        {
            return UpdateCategory(0);
        }

        public ActionResult UpdateCategory(int categoryId)
        {
            CategoryDetail cd;

                if(categoryId != null)
            {
                cd = JsonConvert.DeserializeObject<CategoryDetail>(JsonConvert.SerializeObject(_unitOfWork.GetRepositoryInstance<Tbl_Category>().getFirstorDefault(categoryId)));
            }
                else
            {
                cd = new CategoryDetail();
            }
            return View("UpdateCategory", cd);
        }

        public ActionResult UpdateStatus(int shippingId)
        {
            if (shippingId != null)
            {
                var orden = _unitOfWork.GetRepositoryInstance<Tbl_Shipping>().getFirstorDefault(shippingId);
                if (orden.Status.ToLower().Contains("Pendiente".ToLower()) || orden.Status.ToLower().Contains("PENDING".ToLower()))
                {
                    orden.Status = "En Proceso";
                }
                else if (orden.Status == "En Proceso")
                {
                    orden.Status = "Enviado";
                }
                else if(orden.Status == "Enviado")
                {
                    orden.Status = "Entregado";
                }

                _unitOfWork.GetRepositoryInstance<Tbl_Shipping>().Update(orden);
            }

            return RedirectToAction("Shipping");
        }

        public ActionResult CategoryEdit(int catId)
        {
            return View(_unitOfWork.GetRepositoryInstance<Tbl_Category>().getFirstorDefault(catId));
        }
        [HttpPost]
        public ActionResult CategoryEdit(Tbl_Category tbl)
        {
            _unitOfWork.GetRepositoryInstance<Tbl_Category>().Update(tbl);
            return RedirectToAction("Categories");
        }

        public ActionResult Product()
        {
            return View(_unitOfWork.GetRepositoryInstance<Tbl_Product>().GetProduct());
        }

        public ActionResult Shipping()
        {
            return View(_unitOfWork.GetRepositoryInstance<Tbl_Shipping>().GetAllRecord().Where(x=>x.MiPymeId==1));
        }
        

        public ActionResult ProductEdit(int productId)
        {
            ViewBag.CategoryList = GetCategory();
            return View(_unitOfWork.GetRepositoryInstance<Tbl_Product>().getFirstorDefault(productId));
        }

        public ActionResult ShippingDetail(int shippingId)
        {
            return View(_unitOfWork.GetRepositoryInstance<Tbl_ShippingDetail>().GetAllRecord().Where(x => x.ShippingId == shippingId));
        }

        [HttpPost]
        public ActionResult ProductEdit(Tbl_Product tbl, HttpPostedFileBase file)
        {

            var producto = ctx.Tbl_Product.FirstOrDefault(prod => prod.ProductId == tbl.ProductId);
            producto.ProductName = tbl.ProductName;
            producto.CategoryId = tbl.CategoryId;
            producto.IsActive = tbl.IsActive;
            producto.IsDelete = tbl.IsDelete;
            producto.IsFeatured = tbl.IsFeatured;
            producto.Quantity = tbl.Quantity;
            producto.Price = tbl.Price;
            producto.Existence = tbl.Existence;
            string pic = null;
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/ProductImages/"), pic);
                // file is uploaded
                file.SaveAs(path);
            producto.ProductImage = pic;
            }
            ctx.SaveChanges();
           
            return RedirectToAction("Product");
        }

        public ActionResult ProductAdd()
        {
            ViewBag.CategoryList = GetCategory();
            return View();
        }

        [HttpPost]
        public ActionResult ProductAdd(Tbl_Product tbl, HttpPostedFileBase file)
        {

            string pic = null;
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/ProductImages/"), pic);
                // file is uploaded
                file.SaveAs(path);
            }
            tbl.ProductImage = pic;
            tbl.CreatedDate = DateTime.Now;
            _unitOfWork.GetRepositoryInstance<Tbl_Product>().Add(tbl);
            return RedirectToAction("Product");
        }




    }
}