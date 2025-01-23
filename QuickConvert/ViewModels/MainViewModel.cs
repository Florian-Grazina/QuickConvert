using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuickConvert.API.Enums;
using QuickConvert.Managers;
using QuickConvert.Models;
using System.Globalization;

namespace QuickConvert.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        #region data members
        private bool _isBusy;

        private RateViewModel _rateVM = default!;
        private readonly CultureInfo _cultureInfo;

        private string input = "0";
        private string output = "0";
        #endregion

        #region constructor
        public MainViewModel()
        {
            _isBusy = true;
            _cultureInfo = new CultureInfo("en-US");
            _cultureInfo.NumberFormat.NumberGroupSeparator = " ";
            _isBusy = false;

            Title = Settings.AppName;
            RateVm = GetRateViewModel();
        }
        #endregion

        #region properties
        public AppSettingsManager Settings => AppSettingsManager.Instance;
        public RateManager RateManager => RateManager.Instance;

        public DateTime Date => _rateVM.Date;
        public DateTime ExpirationDate => _rateVM.ExpirationDate;
        public string InputCode => _rateVM.BaseCurrencyCode.ToString();
        public string InputFlag => GetFlagPath(InputCode);
        public string OutputCode => _rateVM.TargetCurrencyCode.ToString();
        public string OutputFlag=> GetFlagPath(OutputCode);
        public double RateAmount => _rateVM.RateAmount;

        public string RateInformation => $"1 {InputCode} = {Math.Round(RateAmount, 2)} {OutputCode}";
        #endregion

        #region observable properties
        [ObservableProperty]
        private string title = default!;

        public RateViewModel RateVm
        {
            get => _rateVM;
            set
            {
                SetProperty(ref _rateVM, value);
                OnPropertyChanged(nameof(Date));
                OnPropertyChanged(nameof(ExpirationDate));
            }
        }

        public string Input
        {
            get => input;
            set
            {
                if (value.StartsWith("0") && value.Length > 1 && value[1] != '.')
                    value = value.TrimStart('0');

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
        private void ForceRefreshRate()
        {
            if (_isBusy)
                return;

            _isBusy = true;
            _rateVM.RefreshRate();
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
                _ = !double.TryParse(input, out double inputAmount);
                double outputAmount = isReversed ? inputAmount / _rateVM.RateAmount : inputAmount * _rateVM.RateAmount;

                string result = outputAmount.ToString("#,#.##", _cultureInfo);
                return string.IsNullOrEmpty(result) ? "0" : result;
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
        private RateViewModel GetRateViewModel()
        {
            Rate rate = RateManager.GetRate();
            return new(rate);
        }

        private string GetFlagPath(string currencyCode)
        {
            return $"Resources/Images/{currencyCode.ToLower()}.png";
        }
        #endregion
    }
}
