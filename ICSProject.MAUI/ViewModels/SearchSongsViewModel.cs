using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICS_Project.BL.Facades;
using ICS_Project.BL.Models;

namespace ICSProject.MAUI.ViewModels;

public partial class SelectableSongModel : ObservableObject
{
    public SongListModel Song { get; set; }
    
    [ObservableProperty]
    private bool _isSelected;
    
    public SelectableSongModel(SongListModel song)
    {
        Song = song;
    }
}

public partial class SearchSongsViewModel : ObservableObject
{
    private readonly SongFacade _songFacade;
    private readonly Guid _playlistId;
    private readonly HashSet<Guid> _songsInPlaylistIds;
    private List<SongListModel> _allAvailableSongs = new();
    
    [ObservableProperty]
    private string _searchText = string.Empty;
    
    [ObservableProperty]
    private int _selectedCount = 0;
    
    public ObservableCollection<SelectableSongModel> FilteredSongs { get; } = new();
    public ObservableCollection<SelectableSongModel> SelectedSongs { get; } = new();
    
    public string EmptyMessage => string.IsNullOrWhiteSpace(SearchText) 
        ? "Start typing to search songs..." 
        : $"No songs found for '{SearchText}'";
    
    public string SelectionMessage => $"{SelectedCount} songs selected";
    
    public event EventHandler<List<SongListModel>>? MultipleSongsSelected;
    public event EventHandler? CloseRequested;
    
    public IRelayCommand<SelectableSongModel> AddSongCommand { get; }
    public IRelayCommand AddSelectedSongsCommand { get; }
    public IRelayCommand CloseCommand { get; }
    
    public SearchSongsViewModel(SongFacade songFacade, Guid playlistId, IEnumerable<Guid> songsInPlaylist)
    {
        _songFacade = songFacade;
        _playlistId = playlistId;
        _songsInPlaylistIds = songsInPlaylist.ToHashSet();
        
        AddSongCommand = new RelayCommand<SelectableSongModel>(OnSongSelected);
        AddSelectedSongsCommand = new RelayCommand(AddSelectedSongs, () => SelectedCount > 0);
        CloseCommand = new RelayCommand(() => CloseRequested?.Invoke(this, EventArgs.Empty));
        
        _ = LoadAvailableSongsAsync();
    }
    
    private async Task LoadAvailableSongsAsync()
    {
        try
        {
            var allSongs = await _songFacade.GetAllAsync();
            _allAvailableSongs = allSongs
                .Where(s => !_songsInPlaylistIds.Contains(s.Id))
                .ToList();
            
            FilterSongs();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading songs: {ex.Message}");
        }
    }
    
    private void FilterSongs()
    {
        try
        {
            var filtered = _allAvailableSongs.AsEnumerable();
            
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var searchLower = SearchText.ToLowerInvariant();
                filtered = filtered.Where(s => 
                    s.Name.ToLowerInvariant().Contains(searchLower) ||
                    s.Genre.ToLowerInvariant().Contains(searchLower));
            }
            
            FilteredSongs.Clear();
            foreach (var song in filtered.Take(50))
            {
                var existingSelected = SelectedSongs.FirstOrDefault(ss => ss.Song.Id == song.Id);
                var selectableSong = new SelectableSongModel(song)
                {
                    IsSelected = existingSelected?.IsSelected ?? false
                };
                FilteredSongs.Add(selectableSong);
            }
            
            OnPropertyChanged(nameof(EmptyMessage));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error filtering songs: {ex.Message}");
        }
    }
    
    private void OnSongSelected(SelectableSongModel? song)
    {
        if (song != null)
        {
            ToggleSongSelection(song);  
        }
    }
    
    private void ToggleSongSelection(SelectableSongModel? song)
    {
        if (song == null) return;
        
        song.IsSelected = !song.IsSelected;
        
        if (song.IsSelected)
        {
            if (!SelectedSongs.Any(s => s.Song.Id == song.Song.Id))
            {
                SelectedSongs.Add(song);
            }
        }
        else
        {
            var toRemove = SelectedSongs.FirstOrDefault(s => s.Song.Id == song.Song.Id);
            if (toRemove != null)
            {
                SelectedSongs.Remove(toRemove);
            }
        }
        
        SelectedCount = SelectedSongs.Count;
        AddSelectedSongsCommand.NotifyCanExecuteChanged();
        OnPropertyChanged(nameof(SelectionMessage));
    }
    
    private void AddSelectedSongs()
    {
        if (SelectedSongs.Count > 0)
        {
            var songList = SelectedSongs.Select(ss => ss.Song).ToList();
            MultipleSongsSelected?.Invoke(this, songList);
        }
    }
    
    private void ClearSelection()
    {
        foreach (var song in FilteredSongs)
        {
            song.IsSelected = false;
        }
        SelectedSongs.Clear();
        SelectedCount = 0;
        AddSelectedSongsCommand.NotifyCanExecuteChanged();
        OnPropertyChanged(nameof(SelectionMessage));
    }
    
    partial void OnSearchTextChanged(string value)
    {
        FilterSongs();
    }
}