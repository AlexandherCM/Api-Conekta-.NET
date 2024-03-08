using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api_OxxoPay.Controllers
{
    public class ConfirmaPagoController : ApiController
    {
        [HttpPost]
        [ActionName("NotificarPago")]
        public string Notificar(Root Order) 
        {
            myStaticData.Orders.Add($"||| Number {myStaticData.Orders.Count + 1}:{Order.id}");
            return $"ID de la orden: {Order.id}";
        }
    }

    public class Data
    {
        public Object @object { get; set; }
    }

    public class Object
    {
        public string id { get; set; }
        public string status { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public PaymentMethod payment_method { get; set; }
    }

    public class PaymentMethod
    {
        public string type { get; set; }
        public int expires_at { get; set; }
    }

    public class Root
    {
        public string id { get; set; }
        public bool livemode { get; set; }
        public string type { get; set; }
        public int created_at { get; set; }
        public Data data { get; set; }
    }

    public static class myStaticData
    {
        public static List<string> Orders {get; set; } = new List<string>();
    }
}
