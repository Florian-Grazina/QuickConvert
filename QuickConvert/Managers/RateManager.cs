using Newtonsoft.Json;
using QuickConvert.Interfaces;
using QuickConvert.Models;

namespace QuickConvert.Managers
{
    public class RateManager : IRateManager
    {
        #region data members
        private readonly HttpClient _client;
        private const string _url = $"https://v6.exchangerate-api.com/v6/{_apiKey}/latest/EUR";
        private const string _apiKey = "7fbbf4d77c8cdceff04d478e";
        #endregion

        #region constructor
        public RateManager()
        {
            _client = new();
        }
        #endregion

        #region public methods
        public async Task<Rate> GetRate()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(_url);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    RateApiObject? rateApi = JsonConvert.DeserializeObject<RateApiObject>(content);

                    Rate? rate = rateApi != null ? new Rate(rateApi) : null;
                    return rate ?? throw new Exception("Failed to deserialize rate object");
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
