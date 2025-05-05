namespace ICSProject.MAUI.Views;

public partial class SongDetailPage : ContentPage
{
    public SongDetailPage(ViewModels.SongDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        viewModel.SaveCompleted += OnSaveCompleted;
    }
    private async void OnSaveCompleted(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}