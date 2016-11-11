using System.Collections.Generic;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Post4Pizza.Exceptions;
using Post4Pizza.PizzaProviders.Model;

namespace Post4Pizza.PizzaProviders
{
    public class CarusoPizzaProvider : IPizzaProvider
    {
        private const string ProviderUrlConst = "https://www.carusopizza.cz/";
        private readonly string loginPage = $"{ProviderUrlConst}cs/login";
        private readonly string opcUrl = $"{ProviderUrlConst}cs/address?back=order-opc.php";

        private readonly string orderUrl = $"{ProviderUrlConst}cs/quick-order";
        private string email;
        private PersistentSessionHttpClient httpClient;

        private string sessionToken;

        public string ProviderName => "CarusoPizzaBrno";
        public string ProviderUrl => ProviderUrlConst;

        public void OrderPizza(string username, string password, List<string> pizzaNames)
        {
            email = username;
            httpClient = LoginToPizzaProvider(username, password);
            pizzaNames.ForEach(AddToCart);

            sessionToken = GetToken();

            var orderData = GetOrderData();
            if ((int) orderData.Summary.total_products != pizzaNames.Count)
            {
                throw new PizzaProviderException($"Failed to order. Count of products after ordering was not equal to count of ordered pizzas. Ordered {pizzaNames.Count}. Products in cart count was {orderData.Summary.total_products}");
            }

            ConfirmOrder(orderData.Summary.Delivery);
        }

        private void ConfirmOrder(Delivery deliveryInfo)
        {
            SendEditCustomer(deliveryInfo);
            SendUpdatePayments();
            SendOrderOpc(deliveryInfo);
            GetConfirmPage();
        }

        private CarusoCustomerData GetOrderData()
        {
            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ajax", "true"),
                new KeyValuePair<string, string>("method", "updateCarrierAndGetPayments"),
                new KeyValuePair<string, string>("id_carrier", "241000"),
                new KeyValuePair<string, string>("recyclable", "0"),
                new KeyValuePair<string, string>("gift", "0"),
                new KeyValuePair<string, string>("gift_message", ""),
                new KeyValuePair<string, string>("token", sessionToken)
            };
            var result = httpClient.PostAndGetString(orderUrl, parameters);
            var data = JsonConvert.DeserializeObject<CarusoCustomerData>(result);
            return data;
        }

        private void SendEditCustomer(Delivery deliveryInfo)
        {
            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ajax", "true"),
                new KeyValuePair<string, string>("method", "editCustomer"),
                new KeyValuePair<string, string>("email", email),
                new KeyValuePair<string, string>("newsletter", "1"),
                new KeyValuePair<string, string>("vat_number", ""),
                new KeyValuePair<string, string>("firstname", deliveryInfo.firstname),
                new KeyValuePair<string, string>("lastname", deliveryInfo.lastname),
                new KeyValuePair<string, string>("address1", deliveryInfo.address1),
                new KeyValuePair<string, string>("postcode", deliveryInfo.postcode),
                new KeyValuePair<string, string>("city", deliveryInfo.city),
                new KeyValuePair<string, string>("phone", deliveryInfo.phone),
                new KeyValuePair<string, string>("id_country", deliveryInfo.id_country),
                new KeyValuePair<string, string>("id_country_invoice", "16"),
                new KeyValuePair<string, string>("customer_lastname", deliveryInfo.lastname),
                new KeyValuePair<string, string>("customer_firstname", deliveryInfo.firstname),
                new KeyValuePair<string, string>("alias", deliveryInfo.alias),
                new KeyValuePair<string, string>("default_alias", "Moje adresa"),
                new KeyValuePair<string, string>("other", ""),
                new KeyValuePair<string, string>("is_new_customer", "0"),
                new KeyValuePair<string, string>("token", sessionToken)
            };
            httpClient.Post(orderUrl, parameters);
        }

        private void SendUpdatePayments()
        {
            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ajax", "true"),
                new KeyValuePair<string, string>("method", "updatePaymentsOnly"),
                new KeyValuePair<string, string>("token", sessionToken)
            };
            httpClient.Post(orderUrl, parameters);
        }

        private void SendOrderOpc(Delivery deliveryInfo)
        {
            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ajax", "true"),
                new KeyValuePair<string, string>("submitAddress", "true"),
                new KeyValuePair<string, string>("type", "delivery"),
                new KeyValuePair<string, string>("firstname", deliveryInfo.firstname),
                new KeyValuePair<string, string>("lastname", deliveryInfo.lastname),
                new KeyValuePair<string, string>("company", ""),
                new KeyValuePair<string, string>("vat_number", ""),
                new KeyValuePair<string, string>("dni", ""),
                new KeyValuePair<string, string>("address1", deliveryInfo.address1),
                new KeyValuePair<string, string>("address2", ""),
                new KeyValuePair<string, string>("postcode", deliveryInfo.postcode),
                new KeyValuePair<string, string>("city", deliveryInfo.city),
                new KeyValuePair<string, string>("id_country", deliveryInfo.id_country),
                new KeyValuePair<string, string>("id_state", ""),
                new KeyValuePair<string, string>("other", ""),
                new KeyValuePair<string, string>("phone", deliveryInfo.phone),
                new KeyValuePair<string, string>("phone_mobile", ""),
                new KeyValuePair<string, string>("alias", deliveryInfo.alias),
                new KeyValuePair<string, string>("default_alias", "Moje adresa"),
                new KeyValuePair<string, string>("id_address", deliveryInfo.id.ToString()),
                new KeyValuePair<string, string>("token", sessionToken)
            };
            httpClient.Post(opcUrl, parameters);
        }

        private void GetConfirmPage()
        {
            var url = "https://www.carusopizza.cz/cs/module/cashonpickup/validation";
            httpClient.Get(url);
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
            var url = $"{ProviderUrlConst}cs/search?search_query={pizzaName}";
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

        private string GetToken()
        {
            var cartPage = httpClient.Get("https://www.carusopizza.cz/cs/quick-order");
            DebugContent.WriteToHtmlFile(cartPage);

            var tokenVariable = Regex.Match(cartPage, "var static_token = '.*'").Value;
            sessionToken = tokenVariable.Substring(20, 32);
            if (sessionToken == null)
            {
                throw new PizzaProviderException("Failed to get static sessionToken");
            }
            return sessionToken;
        }
    }
}