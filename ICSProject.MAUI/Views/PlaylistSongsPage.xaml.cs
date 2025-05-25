using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICS_Project.BL.Models;
using ICSProject.MAUI.ViewModels;

namespace ICSProject.MAUI.Views;

public partial class PlaylistSongsPage : ContentPage
{
    private readonly PlaylistDetailViewModel _viewModel;
 
    
    public PlaylistSongsPage(PlaylistDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        viewModel.EditPlaylistRequested += OnEditPlaylistRequested;
    }

    private async void OnEditPlaylistRequested(object? sender, PlaylistDetailModel playlist)
    {
        if (sender is PlaylistDetailViewModel)
        {
            var editViewModel = Application.Current?.Handler?.MauiContext?.Services.GetService<PlaylistDetailViewModel>();

            if (editViewModel != null)
            {
                await editViewModel.Load(playlist);
                await Navigation.PushAsync(new PlaylistDetailPage(editViewModel));
            }
        }
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadSongsInPlaylistAsync();
    }
}