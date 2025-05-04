using CommunityToolkit.Maui.Views;
using ICS_Project.BL.Models;
using ICSProject.MAUI.ViewModels;
using ICSProject.MAUI.Views;

namespace ICSProject.MAUI;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;
    private readonly IServiceProvider _serviceProvider;

    public MainPage(MainViewModel viewModel, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        _ = viewModel.SongListViewModel.LoadSongsAsync();
        _serviceProvider = serviceProvider;
        _viewModel.SongListViewModel.NavigateToDetailRequested += OnNavigateToDetailRequested;
        _viewModel.SongListViewModel.AddSongRequested += OnAddSongRequested;
    }

    private async void OnNavigateToDetailRequested(object? sender, SongDetailModel song)
    {
        var vm = _serviceProvider.GetRequiredService<ViewModels.SongDetailViewModel>();
        vm.Load(song);

        var detailPage = new SongDetailPage(vm);
        vm.SongChanged += async (_, __) => await _viewModel.SongListViewModel.LoadSongsAsync();
        vm.SaveCompleted += async (_, __) => await Navigation.PopAsync();

        await Navigation.PushAsync(detailPage);
    }
    
    private async void OnAddSongRequested(object? sender, EventArgs e)
    {
        var popup = new AddSongPopup();
        var result = await this.ShowPopupAsync(popup);

        if (result is ValueTuple<string, string, string, string, string> values)
        {
            var (name, author, genre, songUrl, durationText) = values;

            if (int.TryParse(durationText, out var duration))
            {
                await _viewModel.SongListViewModel.AddSongAsync(name, author, genre, songUrl, duration);
                await _viewModel.SongListViewModel.LoadSongsAsync();
            }
        }
    }
}