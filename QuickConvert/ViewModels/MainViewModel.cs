using CommunityToolkit.Mvvm.ComponentModel;
using QuickConvert.Managers;
using QuickConvert.Models;
using QuickConvert.Services;

namespace QuickConvert.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        #region data members
        private readonly RateService _rateService;
        private readonly Rate? _rate;
        #endregion

        #region constructor
        public MainViewModel(RateService rateService)
        {
            _rateService = rateService;
            Title = AppSettingsManager.Instance.AppName;
        }
        #endregion

        #region properties
        public AppSettingsManager Settings => AppSettingsManager.Instance;
        #endregion

        #region observable properties
        [ObservableProperty]
        private string title;

        public string RateValue => _rate?.Value.ToString() ?? string.Empty;
        #endregion

        #region public methods
        public async Task<Rate> LoadRate()
        {
            return Settings.Rate ?? await _rateService.GetRate();
        }
        #endregion
    }
}
