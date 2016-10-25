using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace Post4Pizza
{
    public class PersistentSessionHttpClient
    {
        private readonly HttpClient httpClient;

        public PersistentSessionHttpClient()
        {
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler {CookieContainer = cookieContainer};
            httpClient = new HttpClient(handler);
        }

        public string Get(string url)
        {
            var response = httpClient.GetStringAsync(url).Result;
            return response;
        }

        public string Get(HttpRequestMessage requestMessage)
        {
            var result = httpClient.SendAsync(requestMessage).Result.Content.ReadAsStringAsync().Result;
            return result;
        }

        public string Get(HttpRequestMessage requestMessage, params KeyValuePair<string, string>[] urlParameters)
        {
            if ((urlParameters == null) || (urlParameters.Length == 0))
            {
                throw new ArgumentException(nameof(urlParameters));
            }

            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach (var parameter in urlParameters)
            {
                query[parameter.Key] = parameter.Value;
            }

            string urlWithParameters = $"{requestMessage.RequestUri}?{query}";
            requestMessage.RequestUri = new Uri(urlWithParameters);
            return Get(requestMessage);
        }

        public HttpResponseMessage Post(string url, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            HttpContent content = new FormUrlEncodedContent(parameters);
            var response = httpClient.PostAsync(url, content).Result;
            return response;
        }

        public string PostAndGetString(string url, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            var response = Post(url, parameters);
            var responseContentAsString = response.Content.ReadAsStringAsync().Result;
            return responseContentAsString;
        }

        public HttpResponseMessage Post(string url, IEnumerable<KeyValuePair<string, string>> parameters, List<string> acceptHeaders, IEnumerable<KeyValuePair<string, string>> genericHeaders)
        {
            HttpContent content = new FormUrlEncodedContent(parameters);

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
                Content = content
            };

            foreach (var mediaTypeWithQualityHeaderValue in acceptHeaders)
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaTypeWithQualityHeaderValue));
            }
            foreach (var header in genericHeaders)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            var response = httpClient.SendAsync(request).Result;
            return response;
        }
    }
}