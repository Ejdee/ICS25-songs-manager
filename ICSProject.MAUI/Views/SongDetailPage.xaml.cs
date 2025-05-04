using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICS_Project.BL.Models;
using ICSProject.MAUI.ViewModels;

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
        await Navigation.PopAsync(); // ✅ This is safe — you're in a Page
    }
}