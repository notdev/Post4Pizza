using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Post4Pizza.PizzaProviders;

namespace Post4PizzaTests
{
    [TestClass]
    public class PizzaOrderTests
    {
        [TestMethod]
        public void OrderPizzaTest()
        {
            var order = new CarusoPizzaProvider();
            order.OrderPizza("user@mail.com", "password", new List<string> { "Margherita" });
        }
    }
}