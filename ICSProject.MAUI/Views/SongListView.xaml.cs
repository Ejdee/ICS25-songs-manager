using ICS_Project.BL.Models;
using ICSProject.MAUI.ViewModels;
using CommunityToolkit.Maui.Views;

namespace ICSProject.MAUI.Views;

public partial class SongListView : ContentView
{
    private SongListViewModel? ViewModel => BindingContext as SongListViewModel; 
    
    public SongListView()
    {
        InitializeComponent();
    }

    private void OnTapSongListViewTapped(object? sender, TappedEventArgs e)
    {
        if (BindingContext is SongListModel song && Parent?.BindingContext is MainViewModel mainViewModel)
        {
            var viewModel = mainViewModel.SongListViewModel;
            if (viewModel?.OpenSongUrlCommand?.CanExecute(song.SongUrl) == true)
            {
                viewModel.OpenSongUrlCommand.ExecuteAsync(song.SongUrl);
            }
        }
    }

    private async void OnEditButtonClicked(object? sender, EventArgs e)
    {
        if (BindingContext is SongListModel song && Parent?.BindingContext is MainViewModel mainViewModel)
        {
            var viewModel = mainViewModel.SongListViewModel;
            if (viewModel?.EditSongCommand.CanExecute(song) == true)
            {
                await viewModel.EditSongCommand.ExecuteAsync(song);
            }
        }
    }
}