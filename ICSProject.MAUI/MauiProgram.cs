using ICS_Project.BL.Facades;
using ICS_Project.BL.Mappers;
using ICS_Project.DAL;
using ICS_Project.DAL.UnitOfWork;
using ICSProject.MAUI.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using ICSProject.MAUI.Views;

namespace ICSProject.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "icsproject.db");
        builder.Services.AddDbContextFactory<IcsDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));
        using (var scope = builder.Services.BuildServiceProvider().CreateScope())
        {
            var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<IcsDbContext>>();
            using var db = contextFactory.CreateDbContext();
            db.Database.EnsureCreated();
        }

        builder.Services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
        builder.Services.AddSingleton<SongModelMapper>();
        builder.Services.AddSingleton<SongFacade>();
        builder.Services.AddSingleton<SongListViewModel>();
        builder.Services.AddSingleton<PlaylistListViewModel>();
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddTransient<MainPage>();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit();
        builder.Services.AddTransient<AddSongPopup>();
        builder.Services.AddTransient<ViewModels.SongDetailViewModel>();


        builder.Services.AddSingleton<PlaylistModelMapper>(); 
        builder.Services.AddSingleton<PlaylistSongModelMapper>(); 

        builder.Services.AddSingleton<PlaylistFacade>(); 
        builder.Services.AddSingleton<PlaylistSongFacade>(); 

        builder.Services.AddTransient<PlaylistDetailViewModel>(); 
        builder.Services.AddTransient<PlaylistDetailPage>();
        builder.Services.AddTransient<AddPlaylistPopup>();
        
        


#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}