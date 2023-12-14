using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using ClinicalCoordinationApplication.Model;
using CommunityToolkit.Maui.Storage;

namespace ClinicalCoordinationApplication
{
    public static class MauiProgram
    {
        public static IBusinessLogic BusinessLogic = new BusinessLogic();

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>().UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton(FileSaver.Default); 

            return builder.Build();
        }
    }
}