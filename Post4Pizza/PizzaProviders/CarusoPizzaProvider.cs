using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Post4Pizza.Exceptions;

namespace Post4Pizza.PizzaProviders
{
    public class CarusoPizzaProvider : IPizzaProvider
    {
        private PersistentSessionHttpClient httpClient;

        public string ProviderName => "CarusoPizzaBrno";
        public string ProviderUrl => "http://www.carusopizza.cz/";

        public void OrderPizza(string username, string password, List<string> pizzaNames)
        {
            httpClient = LoginToPizzaProvider(username, password);
            pizzaNames.ForEach(AddToCart);
            GetCartItems();
            ConfirmOrder();
        }

        private void ConfirmOrder()
        {
            throw new System.NotImplementedException();
        }

        private PersistentSessionHttpClient LoginToPizzaProvider(string username, string password)
        {
            var client = new PersistentSessionHttpClient();

            var loginPostForm = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("email", username),
                new KeyValuePair<string, string>("passwd", password),
                new KeyValuePair<string, string>("back", "identity"),
                new KeyValuePair<string, string>("SubmitLogin", "")
            };

            const string loginPage = "https://www.carusopizza.cz/cs/login";

            var response = client.Post(loginPage, loginPostForm);
            var content = response.Content.ReadAsStringAsync().Result;

            // Failed to login - page was not redirected to identity page
            if (!content.Contains("<title>Identity - CarusoPizza</title>"))
            {
                throw new PizzaProviderException($"Failed to login user '{username}'. Check that password is correct, try to log via web on {loginPage}");
            }

            DebugContent.WriteToHtmlFile(content);

            return client;
        }

        private string SearchForPizza(string pizzaName)
        {
            var url = $"http://www.carusopizza.cz/cs/search?search_query={pizzaName}";
            var content = httpClient.Get(url);
            DebugContent.WriteToHtmlFile(content);

            var document = new HtmlDocument();
            document.LoadHtml(content);
            var orderButtonNode = document.DocumentNode.SelectSingleNode("//ul[@class='product-buttons']/li[@class='cart-button-btn']/a[contains(@class,'exclusive')]");
            if (orderButtonNode == null)
            {
                throw new PizzaProviderException($"Failed to find order button for pizza {pizzaName}. It is possible that the pizzeria is closed at the moment.");
            }
            var orderLink = orderButtonNode.Attributes["href"].Value;
            orderLink = orderLink.Replace("&amp;", "&");
            return orderLink;
        }

        private void AddToCart(string pizzaName)
        {
            var url = SearchForPizza(pizzaName);
            var response = httpClient.Get(url);
            DebugContent.WriteToHtmlFile(response);
        }

        private IEnumerable<string> GetCartItems()
        {
            var cartPage = httpClient.Get("https://www.carusopizza.cz/cs/quick-order");
            DebugContent.WriteToHtmlFile(cartPage);
            var pizzaNameNodesXpath = "//p[@class='product-name']/a";
            var document = new HtmlDocument();
            document.LoadHtml(cartPage);
            var pizzaNameNodes = document.DocumentNode.SelectNodes(pizzaNameNodesXpath);
            return pizzaNameNodes.Select(n => n.InnerText);
        }
    }
}