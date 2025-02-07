using QuickConvert.Managers;

namespace QuickConvert
{
    public partial class App : Application
    {
        public App()
        {
            var ok = AppSettingsManager.Instance;
            UserAppTheme = AppTheme.Dark;
            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}
