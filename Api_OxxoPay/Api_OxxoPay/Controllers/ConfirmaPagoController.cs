using Api_OxxoPay.Views;
using Conekta.net.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api_OxxoPay.Controllers
{
    public class ConfirmaPagoController : ApiController
    {
        //[HttpPost]
        //[ActionName("NotificarPago")]
        //public async Task<string> Notificar()
        //{
        //    string json;
        //    using (var reader = new StreamReader(await Request.Content.ReadAsStreamAsync()))
        //    {
        //        json = await reader.ReadToEndAsync();
        //    }

        //    myStaticData.Orders.Add(json);
        //    return "SuccesFull";
        //}


        //[HttpPost]
        //[ActionName("Webhook")]
        //public async Task<string> WebhookEndpoint()
        //{
        //    // Leer y validar la notificación de Conekta
        //    string json;
        //    using (var reader = new StreamReader(await Request.Content.ReadAsStreamAsync()))
        //    {
        //        json = await reader.ReadToEndAsync();
        //    }
        //    myStaticData.Orders.Add(json);
        //    return json;
        //}

        [HttpPost]
        public async Task<IHttpActionResult> Post()
        {
            string requestBody = await Request.Content.ReadAsStringAsync();

            try
            {
                JObject jsonData = JsonConvert.DeserializeObject<JObject>(requestBody);

                string data = jsonData["data"]["object"]["payment_status"].ToString();

                // Verifica si el webhook es del tipo esperado con un estado de pago. - - - - - - - - -
                if (jsonData["data"]?["object"]?["payment_status"] == null)
                    return Ok("WebHook recibido correctamente (Es una estructura diferente a la de de order.paid).");
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

                string paymentStatus = jsonData["data"]["object"]["payment_status"].ToString();
                // Solo proceder si el estado de pago es "paid".
                if (paymentStatus == "paid")
                {
                    string orderId = jsonData["data"]["object"]["id"].ToString();

                    myStaticData.payment_status = orderId;
                    myStaticData.OrderID = paymentStatus;
                    myStaticData.json = requestBody;

                    // Obtiene los metadatos de la orden, si están disponibles.
                    var metadata = jsonData["data"]["object"]["metadata"];
                    if (metadata != null)
                    {
                        // Por ejemplo, obtiene el UserID y PrincipalUser si existen
                        var userId = metadata["UserID"]?.ToString();
                        var principalUser = metadata["PrincipalUser"]?.ToString();

                        myStaticData.UserID = int.Parse(userId);
                        myStaticData.TypeUser = Boolean.Parse(principalUser);
                    }
                }
            }
            catch
            {
                return Ok("ERROR");
            }

            return Ok("WebHook recibido correctamente.");
        }
    }



    //public class Data
    //{
    //    public Object @object { get; set; }
    //}

    //public class Object
    //{
    //    public string id { get; set; }
    //    public string status { get; set; }
    //    public int amount { get; set; }
    //    public string currency { get; set; }
    //    public PaymentMethod payment_method { get; set; }
    //}

    //public class PaymentMethod
    //{
    //    public string type { get; set; }
    //    public int expires_at { get; set; }
    //}

    //public class Root
    //{
    //    public string id { get; set; }
    //    public bool livemode { get; set; }
    //    public string type { get; set; }
    //    public int created_at { get; set; }
    //    public Data data { get; set; }
    //}

    public static class myStaticData
    {
        public static string OrderID { get; set; } = string.Empty;
        public static string payment_status { get; set; } = string.Empty;
        public static int UserID { get; set; }
        public static bool TypeUser { get; set; }
        public static string json { get; set; }
    }
}
