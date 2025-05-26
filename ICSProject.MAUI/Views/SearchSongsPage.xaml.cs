using ICSProject.MAUI.ViewModels;

namespace ICSProject.MAUI.Views;

public partial class SearchSongsPage : ContentPage
{
    public SearchSongsPage(SearchSongsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    public SearchSongsPage()
    {
        InitializeComponent();
    }
}