namespace ICSProject.MAUI;

using Microsoft.Extensions.DependencyInjection;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        MainPage = new NavigationPage(serviceProvider.GetRequiredService<MainPage>());
    }
}