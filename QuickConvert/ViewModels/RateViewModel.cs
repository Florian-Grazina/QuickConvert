using QuickConvert.API.Enums;
using QuickConvert.Managers;

namespace QuickConvert.ViewModels
{
    public class RateViewModel
    {
        public DateTime Date { get; set; }
        public DateTime ExpirationDate { get; set; }

        public BaseCurrencyCode BaseCurrencyCode { get; set; }
        public TargetCurrencyCode TargetCurrencyCode { get; set; }

        public double Rate { get; set; }

        public void RefreshRate()
        {
            Rate = RateManager.Instance.GetRate(BaseCurrencyCode, TargetCurrencyCode);
        }
    }
}
