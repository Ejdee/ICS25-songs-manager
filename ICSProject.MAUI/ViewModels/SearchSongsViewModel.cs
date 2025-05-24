using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICS_Project.BL.Facades;
using ICS_Project.BL.Models;

namespace ICSProject.MAUI.ViewModels;

public partial class SearchSongsViewModel : ObservableObject
{
    private readonly SongFacade _songFacade;
    private readonly Guid _playlistId;
    private readonly HashSet<Guid> _songsInPlaylistIds;
    private List<SongListModel> _allAvailableSongs = new();
    
    [ObservableProperty]
    private string _searchText = string.Empty;
    
    public ObservableCollection<SongListModel> FilteredSongs { get; } = new();
    
    public string EmptyMessage => string.IsNullOrWhiteSpace(SearchText) 
        ? "Start typing to search songs..." 
        : $"No songs found for '{SearchText}'";
    
    public event EventHandler<SongListModel>? SongSelected;
    public event EventHandler? CloseRequested;
    
    public IRelayCommand<SongListModel> AddSongCommand { get; }
    public IRelayCommand CloseCommand { get; }
    
    public SearchSongsViewModel(SongFacade songFacade, Guid playlistId, IEnumerable<Guid> songsInPlaylist)
    {
        _songFacade = songFacade;
        _playlistId = playlistId;
        _songsInPlaylistIds = songsInPlaylist.ToHashSet();
        
        AddSongCommand = new RelayCommand<SongListModel>(OnSongSelected);
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
                FilteredSongs.Add(song);
            }
            
            OnPropertyChanged(nameof(EmptyMessage));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error filtering songs: {ex.Message}");
        }
    }
    
    private void OnSongSelected(SongListModel? song)
    {
        if (song != null)
        {
            SongSelected?.Invoke(this, song);
        }
    }
    
    partial void OnSearchTextChanged(string value)
    {
        FilterSongs();
    }
}