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
        private bool _isBusy;
        private Entry _focusedEntry;

        private RateViewModel _rateVM;
        private readonly CultureInfo _cultureInfo;

        private string baseCurrencyInput = "0";
        private string targetCurrencyOutput = "0";

        private string targetCurrencyInput = string.Empty;
        private string baseCurrencyOutput = string.Empty;
        #endregion

        #region constructor
        public MainViewModel()
        {
            _rateVM = default!;
            _isBusy = true;
            _cultureInfo = new CultureInfo("en-US");
            _cultureInfo.NumberFormat.NumberGroupSeparator = " ";
            _isBusy = false;

            Title = Settings.AppName;
            IsLoaded = false;
        }
        #endregion

        #region properties
        private AppSettingsManager Settings => AppSettingsManager.Instance;
        private RateManager RateManager => RateManager.Instance;

        public DateTime Date => _rateVM.Date;
        public DateTime ExpirationDate => _rateVM.ExpirationDate;
        public string BaseCurrencyCode => _rateVM.BaseCurrencyCode.ToString();
        public string BaseCurrencyFlagImg => GetFlagPath(BaseCurrencyCode);
        public string TargetCurrencyCode => _rateVM.TargetCurrencyCode.ToString();
        public string TargetCurrencyFlagImg => GetFlagPath(TargetCurrencyCode);
        public double RateAmount => _rateVM.RateAmount;

        public string RateInformation => $"1 {BaseCurrencyCode} = {Math.Round(RateAmount, 2)} {TargetCurrencyCode}";
        #endregion

        #region observable properties
        [ObservableProperty]
        private string title = default!;

        [ObservableProperty]
        private bool isLoaded;

        public string BaseCurrencyInput
        {
            get => baseCurrencyInput;
            set
            {
                if (value.StartsWith("0") && value.Length > 1 && value[1] != '.')
                    value = value.TrimStart('0');

                SetProperty(ref baseCurrencyInput, value);

                if (string.IsNullOrEmpty(value))
                {
                    TargetCurrencyOutput = value;
                    return;
                }

                if (!IsLoaded)
                    return;

                BaseCurrencyOutput = string.Empty;
                TargetCurrencyInput = string.Empty;
                Task.Run(async () => TargetCurrencyOutput = await Convert(baseCurrencyInput, false));
            }
        }

        public string TargetCurrencyOutput
        {
            get => targetCurrencyOutput;
            set
            {
                if (value.StartsWith("0") && value.Length > 1 && value[1] != '.')
                    value = value.TrimStart('0');

                SetProperty(ref targetCurrencyOutput, value);
            }
        }

        public string TargetCurrencyInput
        {
            get => targetCurrencyInput;
            set
            {
                if (value.StartsWith("0") && value.Length > 1 && value[1] != '.')
                    value = value.TrimStart('0');

                SetProperty(ref targetCurrencyInput, value);

                if (string.IsNullOrEmpty(value))
                {
                    BaseCurrencyOutput = value;
                    return;
                }

                if (!IsLoaded)
                    return;

                TargetCurrencyOutput = string.Empty;
                BaseCurrencyInput = string.Empty;
                Task.Run(async () => BaseCurrencyOutput = await Convert(targetCurrencyInput, true));
            }
        }

        public string BaseCurrencyOutput
        {
            get => baseCurrencyOutput;
            set
            {
                if (value.StartsWith("0") && value.Length > 1 && value[1] != '.')
                    value = value.TrimStart('0');

                SetProperty(ref baseCurrencyOutput, value);
            }
        }
        #endregion

        #region public methods
        public async Task LoadDataAsync()
        {
            RateViewModel? rateVm = await GetRateViewModel();

            if (rateVm != null)
            {
                IsLoaded = true;
                _rateVM = rateVm;
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
            await _rateVM.ForceRefresh();
            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(ExpirationDate));
            _isBusy = false;
        }

        [RelayCommand]
        private void Clear()
        {
            BaseCurrencyInput = "0";
        }

        [RelayCommand]
        public void SetFocusedEntry(Entry entry)
        {
            _focusedEntry = entry;
        }

        [RelayCommand]
        public void AddDigit(string digit)
        {
            if (_focusedEntry.Text.Contains('.') && digit == ".")
                return;
            _focusedEntry.Text += digit;
        }
        #endregion

        #region public methods
        public async Task<string> Convert(string input, bool isReversed = false)
        {
            try
            {
                if (DateTime.Now > _rateVM.ExpirationDate)
                    await RefreshRate();

                string result = _rateVM.GetRateAmount(_cultureInfo, input, isReversed);

                return result;
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
        private async Task<RateViewModel?> GetRateViewModel()
        {
            Rate? rate = await RateManager.LoadRate();

            if (rate == null)
                return null;

            return new(rate);
        }

        private async Task RefreshRate()
        {
            await _rateVM.RefreshRate();
            OnPropertyChanged(nameof(RateAmount));
            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(ExpirationDate));
        }

        private string GetFlagPath(string currencyCode)
        {
            return $"Resources/Images/{currencyCode.ToLower()}.png";
        }
        #endregion
    }
}
