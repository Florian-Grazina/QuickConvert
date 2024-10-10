
using QuickConvert.Managers;

namespace QuickConvert.Models
{
    public record Rate
    {
        public Currency BaseCurrency { get; }
        public DateTime Date { get; }
        public DateTime ExpirationDate { get; }
        public ConversionRate Values { get; }

        public Rate(RateApiObject rateApiObject)
        {
            Date = DateTime.Now;
            ExpirationDate = Date.AddHours(AppSettingsManager.Instance.NumberOfHoursBeforeRefresh);
            BaseCurrency = Enum.Parse<Currency>(rateApiObject.BaseCode);
            Values = rateApiObject.ConversionRates;
        }
    }
}
