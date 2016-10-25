using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Post4Pizza.Exceptions;
using Post4Pizza.PizzaProviders;

namespace Post4Pizza.Controllers
{
    public class PizzaController : ApiController
    {
        [HttpPost]
        [Route("Api/OrderPizza")]
        public HttpResponseMessage OrderPizza(string pizzaProviderName, string userName, string password, List<string> pizzasToOrder)
        {
            var provider = GetPizzaProvider(pizzaProviderName);
            if (provider == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest) {Content = new StringContent($"Failed to order. Did not find provider '{pizzaProviderName}'")};
            }

            try
            {
                provider.OrderPizza(userName, password, pizzasToOrder);
            }
            catch (PizzaProviderException ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest) {Content = new StringContent($"Failed to order. Error: '{ex.Message}'")};
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError) {Content = new StringContent($"Unhandled error occured. Error: '{ex}'")};
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private IPizzaProvider GetPizzaProvider(string providerName)
        {
            var pizzaProviderInterfaces = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(t => t.GetInterfaces().Contains(typeof(IPizzaProvider)));

            var modules = pizzaProviderInterfaces.Select(type => (IPizzaProvider) Activator.CreateInstance(type));

            var provider = modules.FirstOrDefault(m => m.ProviderName == providerName);
            return provider;
        }
    }
}