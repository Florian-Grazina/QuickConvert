using CommunityToolkit.Mvvm.ComponentModel;
using QuickConvert.API.Enums;
using QuickConvert.Managers;
using QuickConvert.Models;
using System.Globalization;

namespace QuickConvert.ViewModels
{
    public partial class RateViewModel(Rate rate) : ObservableObject
    {
        #region data members
        private readonly Rate _rate = rate;
        #endregion

        #region observable properties
        public DateTime Date => _rate.LastUpdateTime;
        public double RateAmount => _rate.LastRateAmount;
        #endregion

        #region properties
        public DateTime ExpirationDate => _rate.LastUpdateTime.AddHours(AppSettingsManager.Instance.NumberOfHoursBeforeRefresh);

        public BaseCurrencyCode BaseCurrencyCode => _rate.BaseCurrencyCode;
        public TargetCurrencyCode TargetCurrencyCode => _rate.TargetCurrencyCode;
        #endregion

        public async Task RefreshRate()
        {
            await RateManager.Instance.RefreshRate(_rate);
        }

        public async Task ForceRefresh()
        {
            if (_rate != null)
                _rate.LastUpdateTime = DateTime.MinValue;

            await RateManager.Instance.RefreshRate(_rate);
        }

        public string GetRateAmount(CultureInfo cultureInfo, string input, bool isReversed)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            _ = !double.TryParse(input, out double inputAmount);
            double outputAmount = isReversed ? inputAmount / RateAmount : inputAmount * RateAmount;

            string result = outputAmount.ToString("#,#.##", cultureInfo);
            return string.IsNullOrEmpty(result) ? "0" : result;
        }
    }
}
