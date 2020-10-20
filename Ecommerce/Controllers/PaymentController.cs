
using PayPalCheckoutSdk.Orders;
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
        /*public ActionResult PaymentWithPaypal()
        {
            APIContext apicontext = PaypalConfiguration.GetAPIContext();
            try
            {
                string PayerId = Request.Params["PayerID"];
                
                if (string.IsNullOrEmpty(PayerId))
                {
                    string baseURi = Request.Url.Scheme + "://" + Request.Url.Authority +
                        "PaymentWithPaypal/PaymentWithPaypal?";

                    var Guid = Convert.ToString((new Random()).Next(100000000));
                    var createPayment = this.CreatePaymentAsync( baseURi + "guid=" + Guid);

                    var links = createPayment.links.GetEnumerator();
                    string paypalRedirectURL = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectURL = lnk.href;
                        }
                    }
                }
                else
                {
                    var guid = Request.Params["guid"];
                    var executePaymnt = ExecutePayment(apicontext, PayerId, Session[guid] as string);

                    if (executePaymnt.ToString().ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception e)
            {
                return View("FailureView");
                //throw;
            }

            return View("SuccessView");
        }

        private object ExecutePayment( string payerId, string PaymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
             this.payment = new Payment() { id = PaymentId };
             return this.payment.Execute(apicontext, paymentExecution);
    }*/


        private async System.Threading.Tasks.Task CreatePaymentAsync( string redirectURl)
        {

            HttpResponse response;
            //PaypalClient client = new PaypalClient();
            var ItemLIst = new List<PayPalCheckoutSdk.Orders.Item>();

            if (Session["cart"] != "")
            {
                List<Models.Home.Item> cart = (List<Models.Home.Item>)(Session["cart"]);
                foreach (var item in cart)
                {
                    ItemLIst.Add(new PayPalCheckoutSdk.Orders.Item()
                    {
                        Description = "Descripcion",
                        UnitAmount = new Money { CurrencyCode = "USD", Value = item.Product.Price.ToString() },//item.Product.Price,
                        Name = item.Product.ProductName.ToString(),
                        Quantity = item.Product.Quantity.ToString(),
                        Sku = "sku"
                    });
                }

                var order = new OrderRequest()
                {
                    CheckoutPaymentIntent = "CAPTURE",
                    PurchaseUnits = new List<PurchaseUnitRequest>()
                {
                    new PurchaseUnitRequest()
                    {
                        AmountWithBreakdown = new AmountWithBreakdown()
                        {
                            CurrencyCode = "USD",
                            Value = "100.00"
                        }, Items = ItemLIst
                    }
                },
                    ApplicationContext = new ApplicationContext()
                    {
                        ReturnUrl = redirectURl + "&Cancel=true",
                        CancelUrl = redirectURl
                    }
                };

                //var request = new OrdersCreateRequest();
                //request.Prefer("return=representation");
                //request.RequestBody(order);
                //response = await PaypalClient.client().Execute(request);
                //var statusCode = response.StatusCode;
                //Order result = response.Result<Order>();

                /*
                var payer = new Payer() { payment_method = "paypal" };

                var redirUrl = new RedirectUrls()
                {
                    cancel_url = redirectURl + "&Cancel=true",
                    return_url = redirectURl
                };

                var details = new Details()
                {
                    tax = "0",
                    shipping = "0",
                    subtotal = "10"
                };

                var amount = new Amount()
                {
                    currency = "USD",
                    total = "10",//Session["SesTotal"].ToString(),
                    details = details
                };

                var transactionList = new List<Transaction>();
                transactionList.Add(new Transaction()
                {
                    description = "Transaction Desciption",
                    invoice_number = "#100000",
                    amount = amount,
                    item_list = ItemLIst
                });

                this.payment = new Payment()
                {
                    intent = "sale",
                    payer = payer,
                    transactions = transactionList,
                    redirect_urls = redirUrl
                };
                */
            }

            /*return this.payment.Create(apicontext);*/


        }

        

    }
}