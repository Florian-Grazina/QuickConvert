using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuickConvert.Managers;
using QuickConvert.Models;
using System.Globalization;

namespace QuickConvert.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        #region data members
        private readonly RateManager _rateManager;
        private bool _isBusy;

        private string input;
        #endregion

        #region constructor
        public MainViewModel(RateManager rateService)
        {
            _isBusy = true;
            _rateManager = rateService;
            Rate = default!;
            //Title = AppSettingsManager.Instance.AppName;

            Title = "QuickConvert";
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

        public string Input
        {
            get => input;
            set
            {
                SetProperty(ref input, value);
                Convert(value);
            }
        }
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
            try
            {
                _ = !double.TryParse(input, out double result);

                double? outputAmount = result * Rate?.Values.JPY;
                Output = outputAmount.HasValue ? outputAmount.Value.ToString("N2", new CultureInfo("en-EN")) : "0.00";
            }
            catch(Exception ex)
            {
                // manage exception
                Console.WriteLine(ex);
                Output = "An error occured";
                throw;
            }
        }
        #endregion

        #region private methods
        private void SetRate(Rate rate)
        {
            //Settings.Rate = rate;
            Rate = rate;
            RefreshView();
        }

        public void RefreshView()
        {
            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(ExpirationDate));
            Convert(Input);
        }
        #endregion
    }
}
