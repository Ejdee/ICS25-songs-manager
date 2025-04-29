using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICS_Project.BL.Facades;
using ICS_Project.BL.Models;
using System.Threading.Tasks;

namespace ICSProject.MAUI.ViewModels;

public partial class SongListViewModel : ObservableObject
{
    private readonly SongFacade _songFacade;

    public ObservableCollection<SongListModel> Songs { get; } = new();

    public event EventHandler? AddSongRequested;

    public IRelayCommand LoadSongsCommand { get; }
    public IRelayCommand AddSongPopupCommand { get; }

    [ObservableProperty]
    private string searchText;

    public SongListViewModel(SongFacade songFacade)
    {
        _songFacade = songFacade;

        LoadSongsCommand = new RelayCommand(async () => await LoadSongsAsync());
        AddSongPopupCommand = new RelayCommand(() => AddSongRequested?.Invoke(this, EventArgs.Empty));
    }

    public async Task LoadSongsAsync()
    {
        Songs.Clear();
        var songs = await _songFacade.GetAllAsync();

        foreach (var song in songs)
        {
            Songs.Add(song);
        }
    }

    public async Task AddSongAsync(string name, string genre, int durationInSeconds)
    {
        var newSong = new SongDetailModel
        {
            Name = name,
            Genre = genre,
            Description = "TBD",
            DurationInSeconds = TimeSpan.FromSeconds(durationInSeconds),
            Artist = "TBD",
            SongUrl = "TBD"
        };

        await _songFacade.SaveAsync(newSong);
        await LoadSongsAsync();
    }
    [ObservableProperty]
    private SongListModel selectedSong;
    public event EventHandler<SongListModel>? SongSelected;
    partial void OnSelectedSongChanged(SongListModel value)
    {
        if (value is not null)
        {
            // Raise an event or call navigation here
            SelectSong(value);
            SongSelected?.Invoke(this, value);
        }
    }
 
    
    public event EventHandler<SongDetailModel>? NavigateToDetailRequested;

    public async void SelectSong(SongListModel selected)
    {
        var detail = await _songFacade.GetAsync(selected.Id);
        NavigateToDetailRequested?.Invoke(this, detail);
    }
}