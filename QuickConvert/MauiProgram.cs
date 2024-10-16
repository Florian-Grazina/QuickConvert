using Microsoft.Extensions.Logging;
using QuickConvert.Interfaces;
using QuickConvert.Managers;
using QuickConvert.ViewModels;

namespace QuickConvert
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder.Services.AddScoped<MainPage>();
            builder.Services.AddScoped<MainViewModel>();
            builder.Services.AddScoped<RateManager>();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
