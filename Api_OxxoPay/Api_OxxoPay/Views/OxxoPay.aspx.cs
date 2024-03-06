using System;
using Conekta.net.Client;
using Conekta.net.Api;
using Conekta.net.Model;
using static Conekta.net.Model.PlanRequest;

namespace Api_OxxoPay.Views
{
    public partial class OxxoPay : System.Web.UI.Page
    {
        private Configuration _config = new Configuration()
        {
            AccessToken = "key_pq42sBrvzq9u0WnQ6aZvtrg"
        };

        private string _acceptLanguage = "es";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnSend_Click(object sender, EventArgs e)
        {
            var customersApi = new CustomersApi(_config);
            var ordersApi = new OrdersApi(_config);

            var customer = new Customer(
                name: "Alexandher Cordoba",
                email: "alexandhercordoba378@gmail.com",
                phone: "+525591889796"
            );

            CustomerResponse customerResponse = customersApi.CreateCustomer(customer);

            var plansApi = new PlansApi(_config);

            var interval = IntervalEnum.Month;
            var planRequest = new PlanRequest
            {
                Name = "Hola mundo",
                Amount = 100 *100, // Monto en centavos
                Currency = "MXN", // Moneda
                Interval = interval, // Intervalo de facturación
                Frequency = 1, // Frecuencia de facturación
                TrialPeriodDays = 2,
                ExpiryCount = 1
            };

            PlanResponse planResponse = plansApi.CreatePlan(planRequest, _acceptLanguage);

            var subscriptionRequest = new SubscriptionRequest(
                planId: planResponse.Id
            );

            var subscriptionsApi = new SubscriptionsApi(_config);
            SubscriptionResponse subscriptionResponse = subscriptionsApi.CreateSubscription(customerResponse.Id, subscriptionRequest, _acceptLanguage);
        }
    }

}