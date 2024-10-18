using Newtonsoft.Json;
using QuickConvert.API.Enums;
using QuickConvert.API.Models;

namespace QuickConvert.API.Services
{
    public class ApiClient
    {
        #region data members
        private readonly HttpClient _client;
        private const string _url = $"https://v6.exchangerate-api.com/v6/{_apiKey}/latest/";
        private const string _apiKey = "7fbbf4d77c8cdceff04d478e";
        #endregion

        #region constructor
        public ApiClient()
        {
            _client = new();
        }
        #endregion

        #region public methods
        public async Task<ConversionRates> GetConversionRatesByCurrencyCode (BaseCurrencyCode baseCurrencyCode)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(_url + baseCurrencyCode);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    RateApiObject? rateApi = JsonConvert.DeserializeObject<RateApiObject>(content);

                    return rateApi?.ConversionRates ?? throw new Exception("Failed to deserialize rate object");
                }
                throw new Exception("Failed to get rate");
            }
            catch (Exception ex)
            {
                // deal with exception
                throw;
            }
        }
        #endregion
    }
}
