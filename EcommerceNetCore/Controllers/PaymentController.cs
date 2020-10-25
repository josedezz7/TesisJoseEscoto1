using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceNetCore.Controllers
{
    public class PaymentController : Controller
    {
        [HttpGet]
        [Route("Payment/PaypalPagoExitoso/{token}/{PayerID}")]
        public async Task<ActionResult> PaypalPagoExitoso(string token, string PayerID)
        {
            return View("SuccessView");
        }
        // GET: Payment
        [HttpGet]
        public async Task<ActionResult> PaymentWithPaypal()
        {
            try
            {
                string PayerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(PayerId))
                {
                    string baseURi = Request.Url.Scheme + "://" + Request.Url.Authority +
                        "Payment/PaymentWithPaypal?";

                    var Guid = Convert.ToString((new Random()).Next(100000000));
                    var createPayment = await this.CreatePaymentAsync();

                    var links = createPayment.Links.GetEnumerator();
                    string paypalRedirectURL = null;

                    while (links.MoveNext())
                    {
                        LinkDescription lnk = links.Current;
                        Debug.WriteLine(lnk.Rel, lnk.Href);
                        if (lnk.Rel.ToLower().Trim().Equals("approve"))
                        {
                            paypalRedirectURL = lnk.Href;
                            return Redirect(paypalRedirectURL);
                        }
                    }
                }
                /*else
                {
                    var guid = Request.Params["guid"];
                    var executePaymnt = ExecutePayment(apicontext, PayerId, Session[guid] as string);

                    if (executePaymnt.ToString().ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }*/
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return View("FailureView");
                //throw;
            }

            return View("SuccessView");
        }

        /* private object ExecutePayment(string payerId, string PaymentId)
         {
             var paymentExecution = new PaymentExecution() { payer_id = payerId };
             this.payment = new Payment() { id = PaymentId };
             return this.payment.Execute(apicontext, paymentExecution);
         }
 */
        /* public async Task<PayPalHttp.HttpResponse> PaypalPagoExitoso()
         {
             var request = new OrdersCaptureRequest("APPROVED-ORDER-ID");
             request.RequestBody(new OrderActionRequest());
             PayPalHttp.HttpResponse response = await PaypalClient.client().Execute(request);
             var statusCode = response.StatusCode;
             Order result = response.Result<Order>();
             Console.WriteLine("Status: {0}", result.Status);
             Console.WriteLine("Capture Id: {0}", result.Id);
             return response;

         }*/
        public async Task<ActionResult> PaypalPagoFallido()
        {

            return View("FailureView");
        }


        private async Task<Order> CreatePaymentAsync()
        {
            decimal total = 0;
            string baseURi = Request.Url.Scheme + "://" + Request.Url.Authority +
                       "/Payment/";
            PayPalHttp.HttpResponse response;
            //PaypalClient client = new PaypalClient();
            var ItemLIst = new List<PayPalCheckoutSdk.Orders.Item>();

            if (Session["cart"] != "")
            {
                List<Models.Home.Item> cart = (List<Models.Home.Item>)(Session["cart"]);
                foreach (var item in cart)
                {
                    total += ((item.Product.Price * item.Product.Quantity) ?? 0);
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
                            Value = total.ToString(),
                            AmountBreakdown = new AmountBreakdown(){

                             ItemTotal = new Money
                                {
                                    CurrencyCode = "USD",
                                    Value = total.ToString()
                                },
                            },
                        }, Items = ItemLIst
                    }
                },
                    ApplicationContext = new ApplicationContext()
                    {
                        ReturnUrl = baseURi + "PaypalPagoExitoso",
                        CancelUrl = baseURi + "PaypalPagoFallido"
                    }
                };

                var request = new OrdersCreateRequest();
                request.Prefer("return=representation");
                request.RequestBody(order);
                response = await PaypalClient.client().Execute(request);
                var statusCode = response.StatusCode;
                return response.Result<Order>();

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
            return null;
            /*return this.payment.Create(apicontext);*/


        }
    }
}
