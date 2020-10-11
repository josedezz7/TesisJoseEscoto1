using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        public ActionResult PaymentWithPaypal()
        {
            APIContext apicontext = PaypalConfiguration.GetAPIContext();
            try
            {
                string PayerId = Request.Params["PayerId"];
                if (string.IsNullOrEmpty(PayerId))
                {
                    string baseURi = Request.Url.Scheme + "://" + Request.Url.Authority + "PaymentWithPaypal/PaymentWithPaypal?";

                    var Guid = Convert.ToString((new Random()).Next(100000000));
                    var createPayment = this.CreatePayment(apicontext, baseURi + "guid=" + Guid);

                    var links = createPayment.links.GetEnumerator();
                    string paypalRedirectURL = null;

                    while(links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if(lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectURL = lnk.href;
                        }
                    }
                }
                else
                {
                    var guid = Request.Params["guid"];
                    var executePayment = ExecutePayment(apicontext, PayerId, Session[guid] as string);

                    if(executePayment.ToString().ToLower()!="<approved>")
                    {
                        return View("FailuredView");
                    }
                }
            }
            catch(Exception)
            {
                //throw;
            }
        }

        private object ExecutePayment(APIContext apicontext, string payerId, string PaymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId};
            this.payment = new Payment() {id = PaymentId };
            return this.payment.Execute(apicontext, paymentExecution);
        }

        private PayPal.Api.Payment payment;

        private Payment CreatePayment(APIContext apicontext, string redirectURl)
        {
            var ItemList = new ItemList() { items = new List<Item>() };

            if(Session["cart"]!="")
            {
                List<Models.Home.Item> cart = (List<Models.Home.Item>)Session["cart"];
                foreach (var item in cart)
                {
                    ItemList.items.Add(new Item()
                    {
                        name = item.Product.ProductName.ToString(),
                        currency = "TK",
                        price = item.Product.Price.ToString(),
                        quantity = item.Product.Quantity.ToString(),
                        sku = "sku"
                    }); 
                }

                var payer = new Payer() { payment_method = "paypal" };

                var redirUrl = new RedirectUrls()
                {
                    cancel_url = redirectURl + "&Cancel=true",
                    return_url = redirectURl
                };

                var details = new Details()
                {
                    tax = "1",
                    shipping = "1",
                    subtotal = "1"
                };

                var amount = new Amount()
                {
                    currency = "HNL",
                    total = Session["SesTotal"].ToString(),
                    details = details
                };

                var transactionList = new List<Transaction>();
                transactionList.Add(new Transaction()
                {
                    description = "Transaction Desciption",
                    invoice_number = "#100000",
                    amount = amount,
                    item_list = ItemList
                });

                this.payment = new Payment()
                {
                    intent = "sale",
                    payer = payer,
                    transactions = transactionList,
                    redirect_urls = redirUrl
                };
            }

            return this.payment.Create(apicontext);

            
        }
    }
}