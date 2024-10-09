
using QuickConvert.Managers;

namespace QuickConvert.Models
{
    public record Rate
    {
        public DateTime Date { get; } = default!;
        public DateTime ExpirationDate { get; } = default!;
        public decimal Value { get; } = default!;

        public Rate(decimal value)
        {
            Date = DateTime.Now;
            ExpirationDate = Date.AddHours(AppSettingsManager.Instance.NumberOfHoursBeforeRefresh);
            Value = value;
        }
    }
}
