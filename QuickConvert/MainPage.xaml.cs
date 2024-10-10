using QuickConvert.ViewModels;

namespace QuickConvert
{
    public partial class MainPage : ContentPage
    {
        #region data members
        private readonly MainViewModel _viewModel;
        #endregion

        public MainPage(MainViewModel viewModel)
        {
            _viewModel = viewModel;
            BindingContext = _viewModel;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
