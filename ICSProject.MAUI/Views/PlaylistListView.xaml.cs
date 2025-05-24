using ICS_Project.BL.Models;
using ICSProject.MAUI.ViewModels;

namespace ICSProject.MAUI.Views;

public partial class PlaylistListView : ContentView
{
    public PlaylistListView()
    {
        InitializeComponent();
    }

    private async void OnEditButtonClicked(object? sender, EventArgs e)
    {
        if (BindingContext is PlaylistListModel playlist && Parent?.BindingContext is MainViewModel mainViewModel)
        {
            var viewModel = mainViewModel.PlaylistListViewModel;
            if (viewModel?.EditPlaylistCommand?.CanExecute(playlist) == true)
            {
                await viewModel.EditPlaylistCommand.ExecuteAsync(playlist);
            }
        }
    }
}