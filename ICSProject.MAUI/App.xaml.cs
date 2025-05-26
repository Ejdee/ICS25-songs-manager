namespace ICSProject.MAUI;

using Microsoft.Extensions.DependencyInjection;

public partial class App : Application
{
    public static IServiceProvider? ServiceProvider { get; private set; } 
    
    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        MainPage = new NavigationPage(serviceProvider.GetRequiredService<MainPage>());
        ServiceProvider = serviceProvider;
    }
}