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

        private string input;
        private string output;
        #endregion

        #region constructor
        public MainViewModel()
        {
            _isBusy = true;
            _rateManager = RateManager.Instance;
            //Title = AppSettingsManager.Instance.AppName;

            Title = "QuickConvert";

            _rateVM = new()
            {
                Date = DateTime.Now,
                ExpirationDate = DateTime.Now.AddHours(1),
                BaseCurrencyCode = BaseCurrencyCode.EUR,
                TargetCurrencyCode = TargetCurrencyCode.USD
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
        private string title;

        public string Input
        {
            get => input;
            set
            {
                input = value;
                Convert(ref input, ref output, true);
                RefreshView();
            }
        }

        public string Output
        {
            get => output;
            set
            {
                if (value == output)
                    return;

                SetProperty(ref output, value);
                Convert(ref input, ref output, true);
                RefreshView();
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
            _rateVM.Rate = await _rateManager.Refresh(_rateVM.BaseCurrencyCode, _rateVM.TargetCurrencyCode);
            _isBusy = false;
        }
        #endregion

        #region public methods
        public void Convert(ref string input, ref string output, bool isReversed = false)
        {
            try
            {
                if (input == "0")
                {
                    output = "0";
                    input = string.Empty;
                }
                else
                {
                    _ = !double.TryParse(input, out double inputAmount);

                    double outputAmount = isReversed ? inputAmount / _rateVM.Rate : inputAmount * _rateVM.Rate;

                    output = outputAmount.ToString(new CultureInfo("en-EN"));
                    input = inputAmount.ToString(new CultureInfo("en-EN"));
                }
            }
            catch (Exception ex)
            {
                // manage exception
                Console.WriteLine(ex);
                Output = "An error occured";
                throw;
            }
        }
        #endregion

        #region private methods
        public void RefreshView()
        {
            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(ExpirationDate));
            OnPropertyChanged(nameof(Input));
            OnPropertyChanged(nameof(Output));
        }
        #endregion
    }
}
