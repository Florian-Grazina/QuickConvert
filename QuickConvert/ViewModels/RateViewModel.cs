using QuickConvert.API.Enums;

namespace QuickConvert.ViewModels
{
    public class RateViewModel
    {
        public DateTime Date { get; set; }
        public DateTime ExpirationDate { get; set; }

        public BaseCurrencyCode BaseCurrencyCode { get; set; }
        public TargetCurrencyCode TargetCurrencyCode { get; set; }

        public double Rate { get; set; }
    }
}
