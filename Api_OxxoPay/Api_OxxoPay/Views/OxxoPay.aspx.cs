using Api_OxxoPay.Classes;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Api_OxxoPay.Views
{
    public partial class OxxoPay : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected async void BtnSend_Click(object sender, EventArgs e)
        {
            Root root = new Root()
            {
                amount = "200",
                currency = "MXN",
                name = "Alexandher Cordoba",
                email = "alexandhercordoba378@gmail.com",
                phone = "5591889796",
                merchantReferenceCode = "dfscxvsdf",
                oxxoPay = new Pay()
                {
                    expirationNumber = 1,
                    expirationType = "DAY"
                }
            };

            string json = JsonConvert.SerializeObject(root);


            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://gateway-154.netpaydev.com/gateway-ecommerce/v3/oxxopay/reference"),
                Headers =
                {
                    { "accept", "application/json" },
                },
                Content = new StringContent("{\"amount\":\"20\",\"currency\":\"MXN\",\"name\":\"Jon\",\"email\":\"review@netpay.com.mx\",\"phone\":\"8100000222\",\"merchantReferenceCode\":\"referencia-unica-1133\",\"oxxoPay\":{\"expirationNumber\":10,\"expirationType\":\"DAY\"}}")
                {
                    Headers =
        {
            ContentType = new MediaTypeHeaderValue("application/json")
        }
                }
            };
            using (var response = await client.SendAsync(request))
            {
                //response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Response.Write(body);
            }
        }

        public class Pay
        {
            public int expirationNumber { get; set; }
            public string expirationType { get; set; }
        }

        public class Root
        {
            public string amount { get; set; }
            public string currency { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public string merchantReferenceCode { get; set; }
            public Pay oxxoPay { get; set; }
        }
    }
}