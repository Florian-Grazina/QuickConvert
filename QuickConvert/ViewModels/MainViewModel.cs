using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuickConvert.API.Enums;
using QuickConvert.Managers;
using System.Globalization;

namespace QuickConvert.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        #region data members
        private readonly RateManager _rateManager;
        private bool _isBusy;

        private RateViewModel _rateVM;
        private CultureInfo _cultureInfo;

        private string input = "0";
        private string output = "0";
        #endregion

        #region constructor
        public MainViewModel()
        {
            _isBusy = true;
            _rateManager = RateManager.Instance;
            //Title = AppSettingsManager.Instance.AppName;

            _cultureInfo = new CultureInfo("en-US");
            _cultureInfo.NumberFormat.NumberGroupSeparator = " ";

            Title = "QuickConvert";

            _rateVM = new()
            {
                Date = DateTime.Now,
                ExpirationDate = DateTime.Now.AddHours(1),
                BaseCurrencyCode = BaseCurrencyCode.EUR,
                TargetCurrencyCode = TargetCurrencyCode.JPY
            };

            _rateVM.Rate = _rateManager.GetRate(_rateVM.BaseCurrencyCode, _rateVM.TargetCurrencyCode);
            _isBusy = false;

            RefreshView();
        }
        #endregion

        #region properties
        //public AppSettingsManager Settings => AppSettingsManager.Instance;
        public DateTime Date => _rateVM.Date;
        public DateTime ExpirationDate => _rateVM.ExpirationDate;
        #endregion

        #region observable properties
        [ObservableProperty]
        private string title = default!;

        public string Input
        {
            get => input;
            set
            {
                SetProperty(ref input, value);
                output = Convert(input);
                OnPropertyChanged(nameof(Output));
            }
        }

        public string Output
        {
            get => output;
            set
            {
                SetProperty(ref output, value);
                //input = Convert(value);
                //OnPropertyChanged(nameof(Input));
            }
        }
        #endregion

        #region commands
        [RelayCommand]
        private async Task ForceRefreshRate()
        {
            if (_isBusy)
                return;

            _isBusy = true;
            _rateVM.Rate = await _rateManager.Refresh(_rateVM.BaseCurrencyCode, _rateVM.TargetCurrencyCode);
            _isBusy = false;
        }

        [RelayCommand]
        private void Clear()
        {
            Input = "0";
        }
        #endregion

        #region public methods
        public string Convert(string input, bool isReversed = false)
        {
            try
            {
                if (string.IsNullOrEmpty(input) || input == "0" || input == "." || input == ",")
                    return "0";

                else
                {
                    _ = !double.TryParse(input, out double inputAmount);
                    double outputAmount = isReversed ? inputAmount / _rateVM.Rate : inputAmount * _rateVM.Rate;

                    var ok = outputAmount.ToString("#,#.##", _cultureInfo);
                    return ok;
                }
            }
            catch (Exception ex)
            {
                // manage exception
                Console.WriteLine(ex);
                return "An error occured";
            }
        }
        #endregion

        #region private methods
        public void RefreshView()
        {
            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(ExpirationDate));
        }
        #endregion
    }
}
