using Microsoft.AspNetCore.Mvc;
using Checkout;
using Checkout.Common;
using Checkout.Payments;
using Checkout.Payments.Request;
using Checkout.Payments.Sessions;
using Checkout.Sessions;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CheckoutSdkTest.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly string _apiKey = "sk_sbox_lhuepuigeodgom6mw4j7lp2ylif"; // Replace with your actual secret key

        [HttpPost("create-payment-session")]
        public async Task<IActionResult> CreatePaymentSession()
        {
            var api = CheckoutSdk.Builder().StaticKeys()
                .SecretKey(_apiKey) // Using the API key here
                .Environment(Checkout.Environment.Sandbox)
                .Build();

             var CustomerObj = new PaymentCustomerRequest
{
    Email = "customer@example.com",
    Name = "Checkout Sample",
    Phone = new Phone
    {
        CountryCode = "971",
        Number = "547137304"
    }
};

               var threeDsRequest  = new ThreeDsRequest
            {
                Enabled = true, 
            };
            var MetadataObj = new Dictionary<string, object>
            {
    { "order_id", "ORD-ID" },
    { "customer_id", "Example" },
    { "note", "Test payment session with metadata" }};
    

            var request = new PaymentSessionsRequest
            {
                Amount = 1000,
                Currency = Currency.AED,
                PaymentType = PaymentType.Regular,
                ThreeDs = threeDsRequest,
           
                Billing = new BillingInformation
                {
                    Address = new Address
                    {
                        AddressLine1 = "Address",
                        AddressLine2 = "Road",
                        City = "Dubai",
                        State = "Dubai",
                        Zip = "Zip Code",
                        Country = CountryCode.AE
                    },
                    Phone = new Phone
                    {
                        CountryCode = "971",
                        Number = "547137304"
                    }
                },
                Customer = CustomerObj,
                Reference = "ORD- with META",
                Description = "Payment Sessions Description",
                Metadata = MetadataObj,
                EnabledPaymentMethods = new List<PaymentMethodsType>
{
    PaymentMethodsType.Card,
    PaymentMethodsType.Googlepay
},

DisabledPaymentMethods = new List<PaymentMethodsType>
{
    PaymentMethodsType.Applepay
},
                SuccessUrl = "http://localhost:5000/success.html",
                FailureUrl = "http://localhost:5000/success.html"
            };

            try
            {
                
                var response = await api.PaymentSessionsClient().RequestPaymentSessions(request);

Console.WriteLine(JsonConvert.SerializeObject(request, Formatting.Indented));
                // ✅ Print only the fields available in PaymentSessionsResponse
                //Console.WriteLine("✅ Payment Session Created Successfully!");
                //Console.WriteLine($"🔹 Session ID: {response?.Id}");
                //Console.WriteLine("\n🔎 Full API Response:");
                //Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
                // Serialize the response object into a JSON string
        //string parseload = JsonConvert.SerializeObject(response, Formatting.Indented);

        // Return the serialized JSON string as a response
        return Ok(response.Body); // Return response in the API
            }
            catch (CheckoutApiException e)
            {
                Console.WriteLine($"API Error: {e.Message}");
                return StatusCode(500, $"API Error: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unexpected Error: {e.Message}");
                return StatusCode(500, $"Unexpected Error: {e.Message}");
            }
        }
    }
}
