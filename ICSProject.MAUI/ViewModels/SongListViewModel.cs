using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICS_Project.BL.Facades;
using ICS_Project.BL.Models;
using System.Threading.Tasks;
using ICS_Project.BL.Models.Enums;

namespace ICSProject.MAUI.ViewModels;

public partial class SongListViewModel : ObservableObject
{
    private readonly SongFacade _songFacade;

    public ObservableCollection<SongListModel> Songs { get; } = new();
    public ObservableCollection<string> GenreList { get; } = new() { "All", "Pop", "Rock", "Jazz", "Classical" }; // example genres
    public ObservableCollection<SongSortOption> SortOptions { get; } = new()
    {
        SongSortOption.Name,
        SongSortOption.Duration
    };

    [ObservableProperty]
    private string selectedGenre = "All";

    [ObservableProperty]
    private SongSortOption selectedSortOption = SongSortOption.Name;


    public IRelayCommand LoadSongsCommand { get; }
    public IRelayCommand AddSongPopupCommand { get; }
    

    public SongListViewModel(SongFacade songFacade)
    {
        _songFacade = songFacade;

        LoadSongsCommand = new RelayCommand(async () => await LoadSongsAsync());
        AddSongPopupCommand = new RelayCommand(() => AddSongRequested?.Invoke(this, EventArgs.Empty));
    }

    public async Task AddSongAsync(string name, string author, string genre, int durationInSeconds)
    {
        var newSong = new SongDetailModel
        {
            Name = name,
            Genre = genre,
            Description = "TBD",
            DurationInSeconds = TimeSpan.FromSeconds(durationInSeconds),
            Artist = author,
            SongUrl = "song_placeholder.png"
        };
        if (!GenreList.Contains(genre))
        {
            GenreList.Add(genre); // ✅ Add new genre dynamically
        }

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
    [ObservableProperty]
    private string searchText;

    partial void OnSearchTextChanged(string value)
    {
        _ = FilterSongsAsync(value); // Fire-and-forget or handle errors if needed
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
    partial void OnSelectedGenreChanged(string value)
    {
        _ = FilterAndSortSongsAsync();
    }

    partial void OnSelectedSortOptionChanged(SongSortOption value)
    {
        _ = FilterAndSortSongsAsync();
    }

    public async Task LoadSongsAsync()
    {
        _allSongs = (await _songFacade.GetAllAsync()).ToList();
        await FilterAndSortSongsAsync();
        foreach (var song in _allSongs)
        {
            if (!GenreList.Contains(song.Genre))
            {
                GenreList.Add(song.Genre); // ✅ Add new genre dynamically
            }
        }
    }
    
    private List<SongListModel> _allSongs = new();

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
            SongSortOption.Name => filtered.OrderBy(s => s.Name),
            SongSortOption.Duration => filtered.OrderBy(s => s.DurationInSeconds),
            _ => filtered
        };

        Songs.Clear();
        foreach (var song in filtered)
        {
            Songs.Add(song);
        }

        return Task.CompletedTask;
    }

    public event EventHandler? AddSongRequested;
}