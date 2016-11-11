using System.Collections.Generic;
#pragma warning disable 1573

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