using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuickConvert.Managers;
using QuickConvert.Models;

namespace QuickConvert.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        #region data members
        private readonly RateManager _rateManager;
        private bool _isBusy;
        #endregion

        #region constructor
        public MainViewModel(RateManager rateService)
        {
            _isBusy = true;
            _rateManager = rateService;
            Rate = default!;
            //Title = AppSettingsManager.Instance.AppName;

            Title = "QuickConvert";
            Output = "SALUT BITCH";
        }
        #endregion

        #region properties
        //public AppSettingsManager Settings => AppSettingsManager.Instance;
        public DateTime Date => Rate.Date;
        public DateTime ExpirationDate => Rate.ExpirationDate;
        #endregion

        #region observable properties
        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string output;

        [ObservableProperty]
        private Rate rate;
        #endregion

        #region commands
        [RelayCommand]
        public async Task ForceRefreshRate()
        {
            if (_isBusy)
                return;

            _isBusy = true;
            Rate rate = await _rateManager.GetRate();
            SetRate(rate);
            _isBusy = false;
        }
        #endregion

        #region public methods
        public async void OnAppearing()
        {
            //Rate? newRate = Settings.Rate ?? await _rateManager.GetRate();
            Rate newRate = await _rateManager.GetRate();
            SetRate(newRate);
            _isBusy = false;

            RefreshView();
        }

        public void Convert(string input)
        {
            int inputAmount = 0;

            if(!string.IsNullOrEmpty(input))
                inputAmount = int.Parse(input);

            double outputAmount = inputAmount * Rate.Values.JPY;
            Output = outputAmount.ToString();
        }
        #endregion

        #region private methods
        private void SetRate(Rate rate)
        {
            //Settings.Rate = rate;
            Rate = rate;
        }

        public void RefreshView()
        {
            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(ExpirationDate));
            OnPropertyChanged(nameof(Output));
        }
        #endregion
    }
}
