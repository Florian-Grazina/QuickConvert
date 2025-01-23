using QuickConvert.API.Enums;

namespace QuickConvert.Models
{
    public record Rate(BaseCurrencyCode baseCurrency, TargetCurrencyCode targetCurrency)
    {
        public DateTime LastUpdateTime { get; set; } = DateTime.MinValue;
        public double LastRateAmount { get; set; }
        public BaseCurrencyCode BaseCurrencyCode { get; set; } = baseCurrency;
        public TargetCurrencyCode TargetCurrencyCode { get; set; } = targetCurrency;
    }
}
