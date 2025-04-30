using ICS_Project.BL.Facades;
using ICS_Project.BL.Mappers;
using ICS_Project.DAL;
using ICS_Project.DAL.UnitOfWork;
using ICSProject.MAUI.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Views;
using ICSProject.MAUI.ViewModels;
using ICSProject.MAUI.Views;

namespace ICSProject.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        
        // ✅ Register DbContext factory
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "icsproject.db");
        builder.Services.AddDbContextFactory<IcsDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));
        using (var scope = builder.Services.BuildServiceProvider().CreateScope())
        {
            var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<IcsDbContext>>();
            using var db = contextFactory.CreateDbContext();
            db.Database.EnsureCreated(); // ✅ This creates the DB + tables if missing
        }

        builder.Services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
        builder.Services.AddSingleton<SongModelMapper>();
        builder.Services.AddSingleton<SongFacade>();
        builder.Services.AddTransient<SongListViewModel>();
        builder.Services.AddTransient<MainPage>();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit();
        builder.UseMauiCommunityToolkit();
        builder.Services.AddTransient<AddSongPopup>();
        
        builder.Services.AddTransient<SongDetailViewModel>();


#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}