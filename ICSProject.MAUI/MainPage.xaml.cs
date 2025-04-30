using System.Diagnostics;
using CommunityToolkit.Maui.Views;
using ICS_Project.BL.Facades;
using ICS_Project.BL.Mappers;
using ICS_Project.BL.Models;
using ICS_Project.DAL.UnitOfWork;
using ICSProject.MAUI.ViewModels;
using ICSProject.MAUI.Views;

namespace ICSProject.MAUI;

public partial class MainPage : ContentPage
{
    private readonly SongListViewModel _viewModel;
    private readonly IServiceProvider _serviceProvider;

    public MainPage(SongListViewModel viewModel, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        _ = viewModel.LoadSongsAsync();
        _serviceProvider = serviceProvider;
        _viewModel.AddSongRequested += OnAddSongRequested;
        _viewModel.NavigateToDetailRequested += OnNavigateToDetailRequested;
    }
    private async void OnAddSongRequested(object? sender, EventArgs e)
    {
        var popup = new AddSongPopup();
        Debug.WriteLine("Popup should open now");
        var result = await this.ShowPopupAsync(popup);

        if (result is ValueTuple<string, string, string, string> values)
        {
            var (name, author,genre, durationText) = values;

            if (int.TryParse(durationText, out var duration))
                await _viewModel.AddSongAsync(name, author, genre, duration);
        }
    }

    private async void OnNavigateToDetailRequested(object? sender, SongDetailModel song)
    {
        var vm = _serviceProvider.GetRequiredService<SongDetailViewModel>();
        vm.Load(song);

        var detailPage = new SongDetailPage(vm);
        vm.SongChanged += async (_, __) => await _viewModel.LoadSongsAsync(); // ✅ Re-load songs when detail changes
        vm.SaveCompleted += async (_, __) => await Navigation.PopAsync();

        await Navigation.PushAsync(detailPage);
    }
}