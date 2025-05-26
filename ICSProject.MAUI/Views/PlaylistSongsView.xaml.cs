using ICS_Project.BL.Models;
using ICSProject.MAUI.ViewModels;

namespace ICSProject.MAUI.Views;

public partial class PlaylistSongsView
{
    public PlaylistSongsView()
    {
        InitializeComponent();
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (BindingContext is PlaylistDetailViewModel viewModel)
        {
            viewModel.EditPlaylistRequested += OnEditPlaylistRequested;
        }
    }

    private async void OnEditPlaylistRequested(object? sender, PlaylistDetailModel playlist)
    {
        if (sender is PlaylistDetailViewModel detailViewModel)
        {
            var editViewModel = Application.Current?.Handler?.MauiContext?.Services.GetService<PlaylistDetailViewModel>();
            var playlistListViewModel = Application.Current?.Handler?.MauiContext?.Services.GetService<PlaylistListViewModel>();
            var mainViewModel = Application.Current?.Handler?.MauiContext?.Services.GetService<MainViewModel>();

            if (editViewModel != null && playlistListViewModel != null && mainViewModel != null)
            {
                await editViewModel.Load(playlist);
                
                editViewModel.PlaylistChanged += async (_, _) => await playlistListViewModel.LoadPlaylistsAsync();
                
                editViewModel.SaveCompleted += async (_, _) =>
                {
                    await Navigation.PopAsync();
                    await detailViewModel.ReloadPlaylistCommand.ExecuteAsync(playlist);
                };
                
                editViewModel.PlaylistDeleted += async (_, _) => 
                {
                    await playlistListViewModel.LoadPlaylistsAsync();
                    await Navigation.PopAsync();
                    mainViewModel.ShowPlaylistsCommand.Execute(null);
                };
                
                await Navigation.PushAsync(new PlaylistDetailPage(editViewModel));
            }
        }
    }
}