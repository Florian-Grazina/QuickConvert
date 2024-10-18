using QuickConvert.API.Enums;

namespace QuickConvert.API.Models
{
    public class BaseCurrency(BaseCurrencyCode baseCurrencyCode, ConversionRates conversionRates)
    {
        public BaseCurrencyCode BaseCurrencyCode { get; } = baseCurrencyCode;
        public ConversionRates ConversionRates { get; set; } = conversionRates;
    }
}
