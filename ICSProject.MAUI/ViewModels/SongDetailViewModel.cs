using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICS_Project.BL.Facades;
using ICS_Project.BL.Models;

public partial class SongDetailViewModel : ObservableObject
{
    private readonly SongFacade _songFacade;
    public event EventHandler? SaveCompleted;
    public event EventHandler? SongChanged;

    [ObservableProperty]
    private SongDetailModel song;

    public SongDetailViewModel(SongFacade songFacade)
    {
        _songFacade = songFacade;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (Song is null)
        {
            Debug.WriteLine("❌ Song is null when trying to save!");
            return;
        }
        
        Debug.WriteLine($"✅ Saving song: {Song.Name}");
        await _songFacade.SaveAsync(Song);
        SaveCompleted?.Invoke(this, EventArgs.Empty); // ✅ Let the view handle navigation
        SongChanged?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (Song is null || Song.Id == Guid.Empty) return;

        await _songFacade.DeleteAsync(Song.Id);
        SaveCompleted?.Invoke(this, EventArgs.Empty); // ✅ Let the view handle navigation
        SongChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Load(SongDetailModel song)
    {
        Song = song;
    }
}