namespace QuickConvert.Models
{
    public record AppSettings
    {
        public string AppName { get; set; } = "Quick Convert";
        public int NumberOfHoursBeforeRefresh { get; set; } = 4;
        public Rate? Rate { get; set; } = null;
    }
}
