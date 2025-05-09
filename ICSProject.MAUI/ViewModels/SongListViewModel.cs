using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICS_Project.BL.Facades;
using ICS_Project.BL.Models;
using ICS_Project.BL.Models.Enums;

namespace ICSProject.MAUI.ViewModels;

public partial class SongListViewModel : ObservableObject
{
    private readonly SongFacade _songFacade;
    private List<SongListModel> _allSongs = new();

    public ObservableCollection<SongListModel> Songs { get; } = new();
    public ObservableCollection<string> GenreList { get; } = new() { "All", "Pop", "Rock", "Jazz", "Classical" }; // example genres
    public ObservableCollection<SortOptions> SongSortOptions { get; } = new()
    {
        SortOptions.SongName,
        SortOptions.SongDuration
    };

    [ObservableProperty]
    private string _selectedGenre = "All";

    [ObservableProperty] 
    private SortOptions _selectedSortOption = SortOptions.SongName;

    [ObservableProperty]
    private SongListModel _selectedSong = SongListModel.Empty;
    
    [ObservableProperty]
    private string _searchText = string.Empty;

    public IRelayCommand LoadSongsCommand { get; }
    public IRelayCommand AddSongPopupCommand { get; }
    public IAsyncRelayCommand<SongListModel> EditSongCommand { get; }

    public SongListViewModel(SongFacade songFacade)
    {
        _songFacade = songFacade;

        LoadSongsCommand = new RelayCommand(() => _ = ExecuteLoadSongsAsync());
        AddSongPopupCommand = new RelayCommand(() => AddSongRequested?.Invoke(this, EventArgs.Empty));
        EditSongCommand = new AsyncRelayCommand<SongListModel>(NavigateToEditPage);
    }

    public async Task AddSongAsync(string name, string author, string genre, string songUrl, int durationInSeconds)
    {
        var newSong = new SongDetailModel
        {
            Name = name,
            Genre = genre,
            Description = "TBD",
            DurationInSeconds = TimeSpan.FromSeconds(durationInSeconds),
            Artist = author,
            SongUrl = songUrl
        };
        
        if (!GenreList.Contains(genre))
        {
            GenreList.Add(genre);
        }

        await _songFacade.SaveAsync(newSong);
        await LoadSongsAsync();
    }

    public async Task LoadSongsAsync()
    {
        _allSongs = (await _songFacade.GetAllAsync()).ToList();
        await FilterAndSortSongsAsync();
        
        foreach (var song in _allSongs)
        {
            if (!GenreList.Contains(song.Genre))
            {
                GenreList.Add(song.Genre); 
            }
        }
    }
    
    private async Task SelectSong(SongListModel selected)
    {
        var detail = await _songFacade.GetAsync(selected.Id);
        
        if (detail == null)
        {
            return;
        }
        
        NavigateToDetailRequested?.Invoke(this, detail);
    }

    private async Task ExecuteLoadSongsAsync()
    {
        try
        {
            await LoadSongsAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine($"ERROR - loading songs: {e.Message}");
        }
    }
    
    private async Task FilterSongsAsync(string query)
    {
        Songs.Clear();

        var filtered = string.IsNullOrWhiteSpace(query)
            ? await _songFacade.GetAllAsync()
            : await _songFacade.SearchByNameAsync(query);

        foreach (var song in filtered)
            Songs.Add(song);
    }
    
    private Task FilterAndSortSongsAsync()
    {
        var filtered = _allSongs.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            filtered = filtered.Where(s => s.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(SelectedGenre) && SelectedGenre != "All")
        {
            filtered = filtered.Where(s => s.Genre.Equals(SelectedGenre, StringComparison.OrdinalIgnoreCase));
        }

        filtered = SelectedSortOption switch
        {
            SortOptions.SongName => filtered.OrderBy(s => s.Name),
            SortOptions.SongDuration => filtered.OrderBy(s => s.DurationInSeconds),
            _ => filtered
        };

        Songs.Clear();
        foreach (var song in filtered)
        {
            Songs.Add(song);
        }

        return Task.CompletedTask;
    }
    
    private async Task NavigateToEditPage(SongListModel? song)
    {
        if(song != null)
        {
            await SelectSong(song);
        }
    }

    [RelayCommand]
    private static async Task OpenSongUrlAsync(string url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            await Launcher.OpenAsync(url);
        }
    } 
    
    partial void OnSelectedSongChanged(SongListModel value)
    {
        if (value.SongUrl == null)
        {
            return;
        }
        _ = OpenSongUrlAsync(value.SongUrl);
    }

    partial void OnSelectedGenreChanged(string value)
    {
        _ = FilterAndSortSongsAsync();
    }

    partial void OnSelectedSortOptionChanged(SortOptions value)
    {
        _ = FilterAndSortSongsAsync();
    }
    
    partial void OnSearchTextChanged(string value)
    {
        _ = FilterSongsAsync(value);
    }

    public event EventHandler? AddSongRequested;
    public event EventHandler<SongDetailModel>? NavigateToDetailRequested;
}