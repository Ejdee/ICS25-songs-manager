using ICSProject.MAUI.ViewModels;

namespace ICSProject.MAUI.Views;

public partial class PlaylistDetailPage : ContentPage
{
    private readonly PlaylistDetailViewModel _viewModel;
    
    public PlaylistDetailPage(PlaylistDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        
        _viewModel.SaveCompleted += OnSaveCompleted;
    }
    
    
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.SaveCompleted -= OnSaveCompleted;
    }
    
    private async void OnSaveCompleted(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();
    
        if (_viewModel != null)
        {
            await _viewModel.LoadSongsInPlaylistAsync();
        }
    }
}