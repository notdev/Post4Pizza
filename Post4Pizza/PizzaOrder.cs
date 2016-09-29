using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Post4Pizza
{
    public class PizzaOrder
    {
        private readonly string password;
        private readonly string username;
        private string orderToken;

        public PizzaOrder(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        private PersistentSessionHttpClient HttpClient => LoginToPizzaProvider();

        private int EpochTimeStampUtc
        {
            get
            {
                var unixTimestamp = (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                return unixTimestamp;
            }
        }

        private PersistentSessionHttpClient LoginToPizzaProvider()
        {
            var client = new PersistentSessionHttpClient();

            var loginPostForm = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("email", username),
                new KeyValuePair<string, string>("passwd", password),
                new KeyValuePair<string, string>("back", "identity"),
                new KeyValuePair<string, string>("SubmitLogin", "")
            };

            const string loginPage = "http://www.carusopizza.cz/cs/login";

            var response = client.Post(loginPage, loginPostForm);
            var content = response.Content.ReadAsStringAsync().Result;

            // Failed to login - page was not redirected to identity page
            if (!content.Contains("<title>Identity - CarusoPizza</title>"))
            {
                throw new Exception($"Failed to login user '{username}'. Check that password is correct, try to log via web on {loginPage}");
            }

            orderToken = GetToken(content);

            return client;
        }

        private string GetToken(string webPageString)
        {
            const string pattern = "token = '.{32}";
            var token = Regex.Match(webPageString, pattern).Value;
            var cleanToken = token.Substring(9);
            return cleanToken;
        }

        private void AddToCart(string pizzaId)
        {
            var url = $"http://www.carusopizza.cz/?rand={EpochTimeStampUtc}";
            var data = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("controller", "cart"),
                new KeyValuePair<string, string>("add", "1"),
                new KeyValuePair<string, string>("ajax", "true"),
                new KeyValuePair<string, string>("qty", "1"),
                new KeyValuePair<string, string>("id_product", pizzaId),
                new KeyValuePair<string, string>("token", orderToken)
            };
            var acceptHeaders = new List<string> {"application/json"};
            var additionalHeaders = new List<KeyValuePair<string, string>> {new KeyValuePair<string, string>("X-Requested-With", "XMLHttpRequest")};
            var response = HttpClient.Post(url, data, acceptHeaders, additionalHeaders);
            var content = response.Content.ReadAsStringAsync().Result;
        }

        public void OrderPizza()
        {
            AddToCart("29");
        }
    }
}