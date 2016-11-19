using System.Collections.Generic;

namespace Post4Pizza.Controllers
{
    public class Order
    {
        public string PizzaProviderName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<string> Pizzas { get; set; }}
}