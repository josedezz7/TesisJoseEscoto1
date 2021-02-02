using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Ecommerce.DAL;
using Ecommerce.Models;
using Ecommerce.Repository;
using Ecommerce.Models.Home;
using Ecommerce.Models.Products;

namespace Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        dbEcommerceEntities1 ctx = new dbEcommerceEntities1();
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        public ActionResult Index(string search, int? page)
        {
            HomeIndexViewModel model = new HomeIndexViewModel();
            return View(model.CreateModel2(search, 6, page));
        }

        public ActionResult Checkout()
        {
            return View();
        }
        public ActionResult ShippingSearch(string search)
        {
            int orderNumber = 0;
            int.TryParse(search, out orderNumber);
            return View(ctx.Tbl_Shipping.Where(x => x.MiPymeId == 1 && orderNumber == x.ShippingId).ToList());
        }
        public ActionResult CheckoutDetails()
        {
            return View(new OrderViewModel());
        }

        public ActionResult DecreaseQty(int productId)
        {
            if (Session["cart"] != null)
            {
                List<Item> cart = (List<Item>)Session["cart"];
                var product = ctx.Tbl_Product.Find(productId);
                foreach (var item in cart)
                {
                    if (item.Product.ProductId == productId)
                    {
                        int prevQty = item.Quantity;
                        if (prevQty > 0)
                        {
                            cart.Remove(item);
                            cart.Add(new Item()
                            {
                                Product = product,
                                Quantity = prevQty - 1
                            });
                        }
                        break;
                    }
                }
                Session["cart"] = cart;
            }
            return Redirect("Checkout");
        }

        public ActionResult AddToCart(int productId, string url)
        {
            if (Session["cart"] == null)
            {
                List<Item> cart = new List<Item>();
                var product = ctx.Tbl_Product.Find(productId);
                cart.Add(new Item()
                {
                    Product = product,
                    Quantity = 1
                });
                Session["cart"] = cart;
            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart"];
                var count = cart.Count();
                var product = ctx.Tbl_Product.Find(productId);
                for (int i = 0; i < count; i++)
                {
                    if (cart[i].Product.ProductId == productId)
                    {
                        int prevQty = cart[i].Quantity;
                        cart.Remove(cart[i]);
                        cart.Add(new Item()
                        {
                            Product = product,
                            Quantity = prevQty + 1
                        });
                        break;
                    }
                    else
                    {
                        var prd = cart.Where(x => x.Product.ProductId == productId).SingleOrDefault();
                        if (prd == null)
                        {
                            cart.Add(new Item()
                            {
                                Product = product,
                                Quantity = 1
                            });
                        }
                    }
                }
                Session["cart"] = cart;
            }
            return Redirect(url);
        }

        public ActionResult ViewProductDetail(int productId)
        {
            var model = new ProductsDetailsComment
            {
                product = _unitOfWork.GetRepositoryInstance<Tbl_Product>().getFirstorDefault(productId)
            };
                
            return View(model);
        }
            public ActionResult RemoveFromCart(int productId)
            {
                List<Item> cart = (List<Item>)Session["cart"];
                foreach (var item in cart)
                {
                    if (item.Product.ProductId == productId)
                    {
                        cart.Remove(item);
                        break;
                    }
                }
                Session["cart"] = cart;
                return Redirect("Index");
            }

        }
}