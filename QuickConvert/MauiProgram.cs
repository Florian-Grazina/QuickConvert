using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using QuickConvert.Managers;
using QuickConvert.ViewModels;

namespace QuickConvert
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "SolidFA");
                });

            builder.Services.AddScoped<MainPage>();
            builder.Services.AddTransient(provider =>
            {
                MainViewModel viewModel = new();
                Task.Run(() => viewModel.LoadDataAsync()).Wait();
                return viewModel;
            });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
