using System.Windows.Input;
using ICS_Project.BL.Models;
using ICSProject.MAUI.ViewModels;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;

namespace ICSProject.MAUI.Views;

public partial class SongListView : ContentView
{
    private SongListViewModel? ViewModel => BindingContext as SongListViewModel;

    public static readonly BindableProperty ShowEditButtonProperty =
        BindableProperty.Create(nameof(ShowEditButton), typeof(bool), typeof(SongListView), true);

    public bool ShowEditButton
    {
        get => (bool)GetValue(ShowEditButtonProperty);
        set => SetValue(ShowEditButtonProperty, value);
    }
    
    public SongListView()
    {
        InitializeComponent();
    }

    private void OnTapSongListViewTapped(object? sender, TappedEventArgs e)
    {
        string? songUrl = null; 
        
        if (BindingContext is SongDetailModel songDetail)
        {
            songUrl = songDetail.SongUrl;
        }
        else if (BindingContext is SongListModel songList)
        {
            songUrl = songList.SongUrl;
        }

        if (!string.IsNullOrEmpty(songUrl) && Uri.TryCreate(songUrl, UriKind.Absolute, out var uri))
        {
            Launcher.OpenAsync(uri);
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