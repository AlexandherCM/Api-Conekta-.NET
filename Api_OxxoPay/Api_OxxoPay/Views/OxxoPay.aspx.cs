using System;
using System.Collections.Generic;
using Conekta.net.Client;
using Conekta.net.Api;
using Conekta.net.Model;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI;

namespace Api_OxxoPay.Views
{
    public partial class OxxoPay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Controllers.myStaticData.OrderID))
            {
                Response.Write("User ID: " + Controllers.myStaticData.UserID + "<br>");
                Response.Write("Usuario´Principal: " + Controllers.myStaticData.TypeUser + "<br>");
                Response.Write("ID de orden: " + Controllers.myStaticData.OrderID + "<br>");
                Response.Write("Estado: " + Controllers.myStaticData.payment_status + "<br>");
                Response.Write("Objeto: " + Controllers.myStaticData.json + "<br>");
            }
        }

        protected async void BtnSend_Click(object sender, EventArgs e)
        {
            string message;

            OxxoPayApi OxxoPay =
                new OxxoPayApi("Alex Developer", "5591889796", "cordobamolinaalexandher@gmail.com");

            bool estado = await OxxoPay.CreateOrder("PRUEBA ACM", 550);

            if (estado)
            {
                //message = @"<script type='text/javascript'>alert('¡Depósito realizado correctamente!');</script>";
                message = $@"<script type='text/javascript'>alert('Depósito registrado correctamente');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "Alert", message, false);
                return;
            }

            //message = $@"<script type='text/javascript'>alert('{cliente} {amount} {fecha} {id} {numberReference}');</script>";
            message = $@"<script type='text/javascript'>alert('ERROR');</script>";
            ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "Alert", message, false);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Controllers.myStaticData.OrderID = string.Empty;
            Controllers.myStaticData.payment_status = string.Empty;
            Controllers.myStaticData.json = string.Empty;
            Controllers.myStaticData.TypeUser = false;
            Controllers.myStaticData.UserID = 0;
        }
    }

    public class OxxoPayApi 
    {
        private readonly string acceptLanguage = "en";
        private readonly string _key = "Aqui_Va_Tu_Key";

        private Conekta.net.Client.Configuration _config;

        // Expira al medio día del día siguiente (12:00 pm)
        private DateTime _expiresAtDateTime =
            new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1).AddHours(12);

        private CustomersApi _customerApi;
        private OrdersApi _ordersApi;
        private CustomerInfo _userInfor;

         
        public OxxoPayApi(string name, string phone, string email)
        {
            _config = new Conekta.net.Client.Configuration { AccessToken = _key };

            // Instancia de las clases que se comunican con los endPoints del api de Conekta
            _customerApi = new CustomersApi(_config);
            _ordersApi = new OrdersApi(_config);

            _userInfor = new CustomerInfo(
                name: name,
                phone: $"+52{phone}",
                email: email
            );
        }

        public async Task<bool> CreateOrder(string title, int unitPrice)
        {
            // Formato estandar Unix
            long expiresAtUnix = ((DateTimeOffset)_expiresAtDateTime).ToUnixTimeSeconds();

            // Preparar los datos del concepto de pago (Datos del objeto a comprar) - - - - - - - - - - - - - -
            var lineItems = new List<Product>
            {
                new Product(
                    name: title,
                    unitPrice: unitPrice*100, // El precio debe estar en centavos
                    quantity: 1
                )
            };

            // Calcular el total de los lineItems
            int totalOrderAmount = lineItems.Sum(item => item.UnitPrice * item.Quantity);

            // Preparar los lineItems (Cargos sobre la compra) - - - - - - - - - - - - - - - - - - - - - - -
            var charges = new List<ChargeRequest>
            {
                new ChargeRequest(
                    amount: totalOrderAmount, // Asegurar que el monto coincide con el total de los lineItems
                    paymentMethod: new ChargeRequestPaymentMethod(
                        expiresAt: expiresAtUnix,
                        type: "oxxo_cash" // Especificar el tipo de método de pago como Oxxo Cash
                    )
                )
            };

            //List<ShippingRequest> shippingLines = new List<ShippingRequest>()
            //{
            //    new ShippingRequest(
            //        amount: 15 * 100,
            //        carrier: "DHL"
            //   )
            //};

            Dictionary<string, object> metadata = new Dictionary<string, object>
            {
                {"UserID", 1150},
                {"PrincipalUser", true}
            };

            // Preparar el objeto que representa la orden - - - - - - - - - - - - - - - - - - - - - - - - - - -
            var orderRequest = new OrderRequest(
                currency: "MXN",
                customerInfo: new OrderRequestCustomerInfo(_userInfor),
                lineItems: lineItems,
                charges: charges,
                metadata : metadata
            );

            // Intentar crear la orden
            try
            {
                OrderResponse response = await _ordersApi.CreateOrderAsync(orderRequest, acceptLanguage);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}