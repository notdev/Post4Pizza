using System.Collections.Generic;

namespace Post4Pizza.PizzaProviders.Model
{
    internal class CarusoPizza
    {
        public CarusoPizza(string name, string orderUrl)
        {
            Name = name;
            OrderUrl = orderUrl;
        }

        internal string Name { get; }
        internal string OrderUrl { get; }
    }
}