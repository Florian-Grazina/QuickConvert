using QuickConvert.Managers;

namespace QuickConvert
{
    public partial class App : Application
    {
        public App()
        {
            var ok = AppSettingsManager.Instance;
            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}
