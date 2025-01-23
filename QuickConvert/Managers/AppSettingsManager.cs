using Newtonsoft.Json;
using QuickConvert.Models;

namespace QuickConvert.Managers
{
    public class AppSettingsManager
    {
        #region data members
        private static AppSettingsManager? _instance;
        private readonly static object _instanceLock = new();
        private readonly string _configFilePath = Path.Combine(FileSystem.AppDataDirectory, "appsettings.json");
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
                Task.Run(SaveSettings);
            }
        }

        //public Rate Rate
        //{
        //    get => _appSettings.Rate;
        //    set
        //    {
        //        _appSettings.Rate = value;
        //        SaveSettings();
        //    }
        //}
        #endregion

        #region private methods
        private AppSettings LoadSettings()
        {
            AppSettings? appSettings = null;

            if (File.Exists(_configFilePath))
            {
                var ok = File.ReadAllText(_configFilePath);
                appSettings = JsonConvert.DeserializeObject<AppSettings>(ok);
            }

            if(appSettings == null)
            {
                appSettings = new();
                Task.Run(SaveSettings);
            }

            return appSettings;
        }

        private async Task SaveSettings()
        {
            try
            {
                string json = JsonConvert.SerializeObject(_appSettings);

                // Write the JSON string to the file
                await File.WriteAllTextAsync(_configFilePath, json);

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
