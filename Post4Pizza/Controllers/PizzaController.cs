using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Post4Pizza.Controllers
{
    public class PizzaController : ApiController
    {
        [HttpPost]
        [Route("/Api/OrderPizzaNow")]
        public void OrderPizza(string userName, string password, string pizzaCount)
        {
            
        }
    }
}
