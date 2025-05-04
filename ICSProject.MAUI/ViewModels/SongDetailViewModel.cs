using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICS_Project.BL.Facades;
using ICS_Project.BL.Models;

namespace ICSProject.MAUI.ViewModels;

public partial class SongDetailViewModel : ObservableObject
{
    private readonly SongFacade _songFacade;
    public event EventHandler? SaveCompleted;
    public event EventHandler? SongChanged;

    [ObservableProperty]
    private SongDetailModel _song = SongDetailModel.Empty;

    public SongDetailViewModel(SongFacade songFacade)
    {
        _songFacade = songFacade;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        Debug.WriteLine($"✅ Saving song: {Song.Name}");
        await _songFacade.SaveAsync(Song);
        SaveCompleted?.Invoke(this, EventArgs.Empty); 
        SongChanged?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        await _songFacade.DeleteAsync(Song.Id);
        SaveCompleted?.Invoke(this, EventArgs.Empty); 
        SongChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Load(SongDetailModel song)
    {
        Song = song;
    }
}