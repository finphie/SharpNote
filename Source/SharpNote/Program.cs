using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SharpNote;
using SharpNote.Services;
using SharpNote.Views;
using SharpNote.Views.Core.Extensions;
using WpfHosting;

var app = WpfApp.CreateDefaultBuilder()
    .ConfigureServices(static (configuration, services) =>
    {
        // Settings
        services.Configure<AppConfig>(configuration.GetSection(AppConfig.ConfigurationSectionName));
        services.Configure<AssemblyDumpServiceSettings>(configuration.GetSection(AssemblyDumpServiceSettings.ConfigurationSectionName));

        // Library
        services.AddWpfBlazorWebView();

        services.AddSharpNote();
    })
    .ConfigureLogging(static logging =>
    {
        logging.ClearProviders();
        logging.AddDebug();
    })
    .UseWpfApp<App, ShellWindow>()
    .Build();

app.Services.GetRequiredService<Application>().Resources.Add("services", app.Services);
app.Run();
