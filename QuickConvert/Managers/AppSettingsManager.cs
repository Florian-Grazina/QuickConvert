using Newtonsoft.Json;
using QuickConvert.Models;

namespace QuickConvert.Managers
{
    public class AppSettingsManager
    {
        #region data members
        private static AppSettingsManager? _instance;
        private readonly static object _instanceLock = new();
        private readonly string _configFilePath = AppDomain.CurrentDomain.BaseDirectory + "appsettings.json";
        private readonly AppSettings _appSettings;
        #endregion

        #region constructor
        private AppSettingsManager()
        {
            _appSettings = LoadSettings();
        }
        #endregion

        #region properties
        public static AppSettingsManager Instance
        {
            get
            {
                if (_instance == null)
                    lock (_instanceLock)
                        _instance = _instance ?? new AppSettingsManager();
                return _instance;
            }
        }

        public string AppName => _appSettings.AppName;

        public int NumberOfHoursBeforeRefresh
        {
            get => _appSettings.NumberOfHoursBeforeRefresh;
            set
            {
                _appSettings.NumberOfHoursBeforeRefresh = value;
                SaveSettings();
            }
        }

        public Rate Rate
        {
            get => _appSettings.Rate;
            set
            {
                _appSettings.Rate = value;
                SaveSettings();
            }
        }
        #endregion

        #region private methods
        private AppSettings LoadSettings()
        {
            AppSettings? appSettings = null;

            if (File.Exists(_configFilePath))
                appSettings = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(_configFilePath));

            return appSettings ?? new();
        }

        private void SaveSettings()
        {
            try
            {
                File.WriteAllText(_configFilePath, JsonConvert.SerializeObject(_appSettings, Formatting.Indented));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
