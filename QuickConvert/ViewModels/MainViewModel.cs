using CommunityToolkit.Mvvm.ComponentModel;
using QuickConvert.Managers;
using QuickConvert.Models;

namespace QuickConvert.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        #region data members
        private readonly RateManager _rateService;
        private readonly Rate? _rate;
        #endregion

        #region constructor
        public MainViewModel(RateManager rateService)
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

        #endregion

        #region public methods
        public async Task<Rate> LoadRate()
        {
            return Settings.Rate ?? await _rateService.GetRate();
        }
        #endregion
    }
}
