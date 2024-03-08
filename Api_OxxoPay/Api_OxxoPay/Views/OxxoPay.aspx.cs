using System;
using System.Collections.Generic;
using Conekta.net.Client;
using Conekta.net.Api;
using Conekta.net.Model;
using System.Linq;
using Api_OxxoPay.Controllers;

namespace Api_OxxoPay.Views
{
    public partial class OxxoPay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (myStaticData.Orders.Count != 0)
            {
                foreach (var id in myStaticData.Orders)
                {
                    Response.Write(id);
                }
            }
        }

        protected async void BtnSend_Click(object sender, EventArgs e)
        {
            DateTime expiresAtDateTime = DateTime.Now.AddDays(30); // Ejemplo: 30 días a partir de hoy
            long expiresAtUnix = ((DateTimeOffset)expiresAtDateTime).ToUnixTimeSeconds();

            string acceptLanguage = "en";
            Configuration configuration = new Configuration
            {
                AccessToken = "key_pq42sBrvzq9u0WnQ6aZvtrg"
            };

            var customerApi = new CustomersApi(configuration);
            var ordersApi = new OrdersApi(configuration);

            //create customer
            var customer = new Customer(
                name: "Alexandher Cordoba",
                phone: "+525591889796",
                email: "alexandhercordoba378@gmail.com"
            );

            CustomerResponse customerResponse = await customerApi.CreateCustomerAsync(customer);

            // Preparar los lineItems
            var lineItems = new List<Product>
            {
                new Product(
                    name: "Guías",
                    unitPrice: 5200*100, // El precio debe estar en centavos
                    quantity: 1
                )
            };

            // Calcular el total de los lineItems
            int totalOrderAmount = lineItems.Sum(item => item.UnitPrice * item.Quantity);

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

            var customerInfo = new OrderRequestCustomerInfo(
                new CustomerInfoJustCustomerId(customerResponse.Id)
            );

            var orderRequest = new OrderRequest(
                currency: "MXN",
                customerInfo: customerInfo,
                lineItems: lineItems,
                charges: charges
            );

            // Intentar crear la orden
            try
            {
                // Procesar la respuesta, por ejemplo, mostrar el ID de la orden y otros detalles relevantes.
                OrderResponse response = await ordersApi.CreateOrderAsync(orderRequest, acceptLanguage);
                customerApi.DeleteCustomerById(customerResponse.Id);
            }
            catch (Exception ex)
            {
                // Manejar excepción, por ejemplo, registrando o mostrando el mensaje de error.
                Console.WriteLine(ex.Message);
            }
        }

    }
}