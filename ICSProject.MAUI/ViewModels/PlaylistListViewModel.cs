using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICS_Project.BL.Facades;
using ICS_Project.BL.Models;
using ICS_Project.BL.Models.Enums;

namespace ICSProject.MAUI.ViewModels;

public partial class PlaylistListViewModel : ObservableObject
{
    private readonly PlaylistFacade _playlistFacade;

    [ObservableProperty]
    private string _searchText = string.Empty;

    public ObservableCollection<PlaylistListModel> Playlists { get; } = new();

    public ObservableCollection<SortOptions> PlaylistSortOptions { get; } = new()
    {
        SortOptions.PlaylistName,
        SortOptions.PlaylistSongCount,
        SortOptions.PlaylistDuration
    };

    [ObservableProperty] 
    private SortOptions _selectedSortOption = SortOptions.PlaylistName;

    public IAsyncRelayCommand LoadPlaylistsCommand { get; }
    public IRelayCommand AddPlaylistPopupCommand { get; }
    public IAsyncRelayCommand<PlaylistListModel> EditPlaylistCommand { get; }

    public event EventHandler? AddPlaylistRequested;
    public event EventHandler<PlaylistDetailModel>? NavigateToDetailRequested;

    public PlaylistListViewModel(PlaylistFacade playlistFacade)
    {
        _playlistFacade = playlistFacade;

        LoadPlaylistsCommand = new AsyncRelayCommand(LoadPlaylistsAsync);
        AddPlaylistPopupCommand = new RelayCommand(() => AddPlaylistRequested?.Invoke(this, EventArgs.Empty));
        EditPlaylistCommand = new AsyncRelayCommand<PlaylistListModel>(NavigateToEditPage);
        
        _ = LoadPlaylistsAsync();
    }

    public async Task LoadPlaylistsAsync()
    {
        try
        {
            IEnumerable<PlaylistListModel> playlists;
            
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                playlists = await _playlistFacade.SearchByNameAsync(SearchText);
            }
            else
            {
                playlists = await _playlistFacade.GetSortedAsync(_selectedSortOption);
            }
            
            Playlists.Clear();
            foreach (var playlist in playlists)
            {
                Playlists.Add(playlist);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"ERROR - loading playlists: {e.Message}");
        }
    }

    public async Task AddPlaylistAsync(string name, string description)
    {
        var newPlaylist = new PlaylistDetailModel
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            DurationInSeconds = TimeSpan.Zero,
            SongCount = 0
        };

        await _playlistFacade.SaveAsync(newPlaylist);
        await LoadPlaylistsAsync();
    }

    private async Task SelectPlaylist(PlaylistListModel selected)
    {
        try
        {
            var detail = await _playlistFacade.GetAsync(selected.Id);
            
            if (detail == null)
            {
                return;
            }
            
            NavigateToDetailRequested?.Invoke(this, detail);
        }
        catch (Exception e)
        {
            Console.WriteLine($"ERROR - selecting playlist: {e.Message}");
        }
    }

    private async Task NavigateToEditPage(PlaylistListModel? playlist)
    {
        if(playlist != null)
        {
            await SelectPlaylist(playlist);
        }
    }

    partial void OnSearchTextChanged(string value)
    {
        _ = LoadPlaylistsAsync();
    }

    partial void OnSelectedSortOptionChanged(SortOptions value)
    {
        _ = LoadPlaylistsAsync();
    }
}