using CommunityToolkit.Mvvm.ComponentModel;
using QuickConvert.Managers;

namespace QuickConvert
{
    public partial class MainViewModel : ObservableObject
    {
        #region data members
        #endregion

        #region constructor
        public MainViewModel()
        {
            Title = AppSettingsManager.Instance.AppName;
        }
        #endregion

        #region observable properties
        [ObservableProperty]
        private string title;
        #endregion
    }
}
