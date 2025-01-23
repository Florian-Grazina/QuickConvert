using QuickConvert.API.Enums;
using QuickConvert.Managers;
using QuickConvert.Models;

namespace QuickConvert.ViewModels
{
    public class RateViewModel(Rate rate)
    {
        #region data members
        private readonly Rate _rate = rate;
        #endregion

        #region properties
        public DateTime Date => _rate.LastUpdateTime;
        public DateTime ExpirationDate => _rate.LastUpdateTime.AddHours(AppSettingsManager.Instance.NumberOfHoursBeforeRefresh);

        public BaseCurrencyCode BaseCurrencyCode => _rate.BaseCurrencyCode;
        public TargetCurrencyCode TargetCurrencyCode => _rate.TargetCurrencyCode;

        public double RateAmount => _rate.LastRateAmount;
        #endregion

        public void RefreshRate()
        {
            RateManager.Instance.RefreshRate(_rate);
        }
    }
}
