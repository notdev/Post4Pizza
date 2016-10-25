using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Post4Pizza.PizzaProviders;

namespace Post4Pizza.Controllers
{
    public class PizzaController : ApiController
    {
        [HttpPost]
        [Route("Api/OrderPizza")]
        public void OrderPizza(string pizzaProviderName, string userName, string password, List<string> pizzasToOrder)
        {
            var provider = GetPizzaProvider(pizzaProviderName);
            provider.OrderPizza(userName, password, pizzasToOrder);
        }

        private IPizzaProvider GetPizzaProvider(string providerName)
        {
            var type = typeof(IPizzaProvider);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));
            var pizzaProviders = types as IEnumerable<IPizzaProvider>;
            return pizzaProviders.FirstOrDefault(p => p.ProviderName == providerName);
        }
    }
}