using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Post4Pizza.Exceptions;
using Post4Pizza.PizzaProviders;
using Serilog;

namespace Post4Pizza.Controllers
{
    public class OrderPizzaController : ApiController
    {
        [HttpPost]
        [Route("Api/OrderPizza")]
        public HttpResponseMessage OrderPizza(string pizzaProviderName, string userName, string password, List<string> pizzasToOrder)
        {
            var requestIp = HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : string.Empty;
            Log.Information($"Order received from IP {requestIp}.");

            if (string.IsNullOrEmpty(pizzaProviderName))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Empty provider name"));
            }
            if (string.IsNullOrEmpty(userName))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Empty username"));
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Empty username"));
            }
            if ((pizzasToOrder == null) || (pizzasToOrder.Count == 0))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No pizzas in request"));
            }
            try
            {
                var provider = FindPizzaProvider(pizzaProviderName);
                if (provider == null)
                {
                    string errorMessage = $"Failed to order. Did not find provider '{pizzaProviderName}'";
                    Log.Error(errorMessage);
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, errorMessage);
                }

                provider.OrderPizza(userName, password, pizzasToOrder);

                Log.Information($"Pizza ordered OK from provider {provider.ProviderName}. Pizzas ordered: {string.Join(", ", pizzasToOrder)}");
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (PizzaProviderException ex)
            {
                Log.Error(ex.ToString());
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Failed to order. Error: '{ex.Message}'");
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return new HttpResponseMessage(HttpStatusCode.InternalServerError) {Content = new StringContent($"Unhandled error occured. Error: '{ex}'")};
            }
        }

        [HttpGet]
        [Route("Api/OrderPizza/Providers")]
        public Dictionary<string, string> GetAvailablePizzaProviders()
        {
            var allproviders = GetAllPizzaProviders();
            return allproviders.ToDictionary(provider => provider.ProviderName, provider => provider.ProviderUrl);
        }


        private IEnumerable<IPizzaProvider> GetAllPizzaProviders()
        {
            var pizzaProviderInterfaces = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(t => t.GetInterfaces().Contains(typeof(IPizzaProvider)));

            var providers = pizzaProviderInterfaces.Select(type => (IPizzaProvider) Activator.CreateInstance(type));
            return providers;
        }

        private IPizzaProvider FindPizzaProvider(string providerName)
        {
            var allProviders = GetAllPizzaProviders();
            var provider = allProviders.FirstOrDefault(m => m.ProviderName == providerName);
            return provider;
        }
    }
}