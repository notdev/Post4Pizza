using System.Collections.Generic;

namespace Post4Pizza.PizzaProviders
{
    internal interface IPizzaProvider
    {
        string ProviderName { get; }
        string ProviderUrl { get; }

        /// <summary>
        ///     Order list of Pizzas
        /// </summary>
        /// <param name="pizzaNames">List of Pizzas to order. There can be duplicates for more than one pizza</param>
        void OrderPizza(string username, string password, List<string> pizzaNames);
    }
}