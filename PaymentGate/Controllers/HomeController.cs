using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PaymentGate.Models;
using Stripe.Checkout;
using System.Diagnostics;

namespace PaymentGate.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly StripeSettings _stripeSettings;

        public string SessionId { get; set; }

        public HomeController(IOptions<StripeSettings> stripeSettings)
        {
            _stripeSettings = stripeSettings.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /*[HttpPost]
        [Route("api/Home/PaymentIntentAsync")]
        public async Task<IActionResult> PaymentIntentAsync([FromBody] PaymentModel model)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.stripe.com/v1/payment_intents");
            request.Headers.Add("Authorization", "Bearer Your_Secret_Key");
            var collection = new List<KeyValuePair<string, string>>();
            collection.Add(new("amount", model.amount));
            collection.Add(new("currency", model.currency));
            collection.Add(new("payment_method_types[]", model.payment_method_types[0]));
            var content = new FormUrlEncodedContent(collection);
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("success");
        }*/

        public IActionResult CreateCheckoutSession(string amount)
        {
            var currency = "inr";
            var successUrl = "https://localhost:44321/Home/success";
            var cancelUrl = "https://localhost:44321/Home/cancel";

            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
                {
                    new Stripe.Checkout.SessionLineItemOptions
                    {
                        PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                        {
                            Currency = currency,
                            UnitAmount = Convert.ToInt64(amount) * 100, // Use Int64 for UnitAmount
                            ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Name",
                                Description = "Description"
                            }
                        },
                        Quantity = 1
                    }
                },
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl
            };

            var service = new SessionService();
            var session = service.Create(options);
            SessionId = session.Id;

            return Redirect(session.Url);
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Cancel()
        {
            return View("Index");
        }
    }
}