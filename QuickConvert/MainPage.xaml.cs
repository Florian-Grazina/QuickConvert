using QuickConvert.ViewModels;

namespace QuickConvert
{
    public partial class MainPage : ContentPage
    {
        #region data members
        private MainViewModel _viewModel;
        #endregion

        public MainPage(MainViewModel viewModel)
        {
            _viewModel = viewModel;
            BindingContext = _viewModel;
            InitializeComponent();
        }
    }
}
