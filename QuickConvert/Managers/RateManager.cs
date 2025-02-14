
using QuickConvert.API.Enums;
using QuickConvert.API.Models;
using QuickConvert.API.Services;
using QuickConvert.Models;
using System.Reflection;

namespace QuickConvert.Managers
{
    public class RateManager
    {
        #region data members
        private static RateManager _instance = default!;
        private static readonly object lockObject = new();

        private readonly ApiClient _apiClient;
        private readonly List<BaseCurrency> _baseCurrencies;
        #endregion

        #region constructor
        private RateManager()
        {
            _apiClient = new ApiClient();
            _baseCurrencies = [];
        }

        public static RateManager Instance
        {
            get
            {
                if (_instance == null)
                    lock (lockObject)
                        _instance ??= new RateManager();

                return _instance;
            }
        }
        #endregion

        #region public methods
        //public async Task Init() => await LoadBaseCurrencies();

        public async Task<Rate?> LoadRate()
        {
            Rate? rate = AppSettingsManager.Instance.Rate;

            rate = null;

            await RefreshRate(rate);

            return rate;
        }

        public async Task RefreshRate(Rate? rate)
        {
            //if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            if (true)
                return;

            if(rate == null)
                rate = new(BaseCurrencyCode.EUR, TargetCurrencyCode.JPY);

            if (GetNextRefreshTime(rate) > DateTime.Now)
                return;

            await LoadBaseCurrencies();
            double newAmount = GetRateAmount(rate.BaseCurrencyCode, rate.TargetCurrencyCode);
            rate.LastRateAmount = newAmount;
            rate.LastUpdateTime = DateTime.Now;
            AppSettingsManager.Instance.SaveNewRate(rate);
        }

        public double GetRateAmount(BaseCurrencyCode baseCurrencyCode, TargetCurrencyCode targetCurrencyCode)
        {
            try
            {
                BaseCurrency baseCurrency = _baseCurrencies.Find(baseCurrency => baseCurrency.BaseCurrencyCode == baseCurrencyCode) ?? throw new Exception($"Impossible to find the currency code '{baseCurrencyCode}'");

                PropertyInfo? property = typeof(ConversionRates).GetProperty(targetCurrencyCode.ToString());
                if (property != null)
                    return (double)property.GetValue(baseCurrency.ConversionRates)!;

                throw new Exception($"Impossible to find the currency code '{targetCurrencyCode}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public DateTime GetNextRefreshTime(Rate rate) => rate.LastUpdateTime.AddHours(AppSettingsManager.Instance.NumberOfHoursBeforeRefresh);
        #endregion

        #region private methods
        private async Task LoadBaseCurrencies()
        {
            foreach (BaseCurrencyCode baseCurrencyCode in Enum.GetValues<BaseCurrencyCode>())
            {
                ConversionRates conversionRates = await _apiClient.GetConversionRatesByCurrencyCode(baseCurrencyCode);
                BaseCurrency baseCurrency = new(baseCurrencyCode, conversionRates);
                _baseCurrencies.Add(baseCurrency);
            }
        }

        #endregion
    }
}
