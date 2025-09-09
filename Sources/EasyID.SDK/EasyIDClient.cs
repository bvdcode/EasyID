using System;
using System.Net.Http;
using System.Collections.Generic;

namespace EasyID.SDK
{
    /// <summary>
    /// EasyIDClient is a client to access the EasyID API.
    /// </summary>
    public class EasyIDClient : IEasyIDClient
    {
        private readonly Guid _apiKey;
        private readonly HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// Initializes a new instance of the EasyIDClient class with the specified base URL.
        /// </summary>
        public EasyIDClient(string baseUrl, Guid apiKey)
        {
            if (apiKey == Guid.Empty)
            {
                throw new ArgumentException("API key is required.", nameof(apiKey));
            }
            _apiKey = apiKey;
            _httpClient.BaseAddress = new Uri(baseUrl);
            string currentAssembly = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", currentAssembly);
        }
    }
}