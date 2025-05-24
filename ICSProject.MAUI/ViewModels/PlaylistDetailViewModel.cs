using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICS_Project.BL.Facades;
using ICS_Project.BL.Models;
using ICSProject.MAUI.Views;

namespace ICSProject.MAUI.ViewModels;

public partial class PlaylistDetailViewModel : ObservableObject
{
    private readonly PlaylistFacade _playlistFacade;
    private readonly SongFacade _songFacade;
    private readonly PlaylistSongFacade _playlistSongFacade;

    [ObservableProperty]
    private PlaylistDetailModel _playlist = PlaylistDetailModel.Empty;

    [ObservableProperty]
    private ObservableCollection<SongDetailModel> _songsInPlaylist = new();
    
    public event EventHandler? SaveCompleted;
    public event EventHandler? PlaylistChanged;
    
    public PlaylistDetailViewModel(
        PlaylistFacade playlistFacade, 
        SongFacade songFacade,
        PlaylistSongFacade playlistSongFacade)
    {
        _playlistFacade = playlistFacade ?? throw new ArgumentNullException(nameof(playlistFacade));
        _songFacade = songFacade ?? throw new ArgumentNullException(nameof(songFacade));
        _playlistSongFacade = playlistSongFacade ?? throw new ArgumentNullException(nameof(playlistSongFacade));
        
        Playlist = PlaylistDetailModel.Empty;
    }

    public async Task Load(PlaylistDetailModel playlist)
    {
        Playlist = playlist ?? PlaylistDetailModel.Empty;
        await LoadSongsInPlaylistAsync();
    }

    public async Task LoadSongsInPlaylistAsync()
    {
    
        SongsInPlaylist.Clear();
    
        if (Playlist.Songs != null)
        {
            foreach (var playlistSong in Playlist.Songs)
            {
                try
                {
                    var songDetail = await _songFacade.GetAsync(playlistSong.SongId);
                    if (songDetail != null)
                    {
                        SongsInPlaylist.Add(songDetail);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading song details: {ex.Message}");
                }
            }
        }
        OnPropertyChanged(nameof(SongsInPlaylist));
        
        ApplySongFilter();
    }


    [RelayCommand]
    private async Task SaveAsync()
    {
        
        try
        { 
            var playlistToSave = new PlaylistDetailModel
            {
                Id = Playlist.Id,
                Name = Playlist.Name,
                Description = Playlist.Description,
                DurationInSeconds = Playlist.DurationInSeconds,
                SongCount = Playlist.SongCount
            };
            
            await _playlistFacade.SaveAsync(playlistToSave);
            PlaylistChanged?.Invoke(this, EventArgs.Empty);
            SaveCompleted?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to save playlist: {ex.Message}", "OK");
        }
    }

    
    [RelayCommand]
    private async Task DeleteAsync()
    {
        try
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Delete Playlist", 
                $"Are you sure you want to delete '{Playlist.Name}'?", 
                "Yes", "No");

            if (confirm)
            {
                try
                {
                    await _playlistFacade.DeleteAsync(Playlist.Id);
                    PlaylistChanged?.Invoke(this, EventArgs.Empty);
                    SaveCompleted?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"Failed to delete playlist: {ex.Message}", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DeleteAsync: {ex.Message}\n{ex.StackTrace}");
        }
    }
    
    [RelayCommand]
    private async Task AddSongToPlaylistAsync()
    {
        await ShowLiveSearchModal(); // Priamo otvorí live search modal
    }
    
private async Task AddSelectedSongToPlaylist(SongListModel selectedSong)
{
    try
    {
        var songDetail = await _songFacade.GetAsync(selectedSong.Id);
        if (songDetail == null)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Failed to get song details", "OK");
            return;
        }

        var playlistSong = new PlaylistSongListModel
        {
            Id = Guid.NewGuid(),
            SongId = songDetail.Id,
            SongName = songDetail.Name,
            SongDurationInSeconds = songDetail.DurationInSeconds
        };
        
        await _playlistSongFacade.SaveAsync(playlistSong, Playlist.Id);
        await ReloadPlaylistAsync();
        
        PlaylistChanged?.Invoke(this, EventArgs.Empty);
        
       // await Application.Current.MainPage.DisplayAlert("Success", $"'{songDetail.Name}' added to playlist", "OK");
    }
    catch (Exception ex)
    {
        await Application.Current.MainPage.DisplayAlert("Error", $"Failed to add song to playlist: {ex.Message}", "OK");
    }
}

    private async Task ReloadPlaylistAsync()
    {
        var refreshedPlaylist = await _playlistFacade.GetAsync(Playlist.Id);
        if (refreshedPlaylist != null)
        {
            Playlist = refreshedPlaylist;
            await LoadSongsInPlaylistAsync();
        }
    }
    
    
    [RelayCommand]
    private async Task RemoveSongFromPlaylistAsync(SongDetailModel song)
    {
        if (song == null) return;
    
        bool confirm = await Application.Current.MainPage.DisplayAlert(
            "Remove Song", 
            $"Are you sure you want to remove '{song.Name}' from this playlist?", 
            "Yes", "No");
    
        if (confirm)
        {
            try
            {
                // Find the playlist song
                var playlistSong = Playlist.Songs?.FirstOrDefault(ps => ps.SongId == song.Id);
                if (playlistSong != null)
                {
                    await _playlistSongFacade.DeleteAsync(playlistSong.Id);
                    
                    var updatedSongs = new List<PlaylistSongListModel>(Playlist.Songs.Where(s => s.Id != playlistSong.Id));
                    
                    // Update the playlist with the removed song
                    Playlist = new PlaylistDetailModel
                    {
                        Id = Playlist.Id,
                        Name = Playlist.Name,
                        Description = Playlist.Description,
                        DurationInSeconds = Playlist.DurationInSeconds,
                        SongCount = Playlist.SongCount - 1,
                        Songs = new ObservableCollection<PlaylistSongListModel>(updatedSongs)
                    };
                    
                    SongsInPlaylist.Remove(song);
                    
                    var playlistToUpdate = new PlaylistDetailModel
                    {
                        Id = Playlist.Id,
                        Name = Playlist.Name,
                        Description = Playlist.Description,
                        DurationInSeconds = Playlist.DurationInSeconds,
                        SongCount = Playlist.SongCount
                    };
                    
                    await _playlistFacade.SaveAsync(playlistToUpdate);
                    
                    PlaylistChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to remove song from playlist: {ex.Message}", "OK");
            }
        }
    }
    [ObservableProperty]
    private string _songSearchText = string.Empty;

    [ObservableProperty]
    private ObservableCollection<SongDetailModel> _filteredSongsInPlaylist = new();

    partial void OnSongSearchTextChanged(string value)
    {
        ApplySongFilter();
    }

    private void ApplySongFilter()
    {
        if (string.IsNullOrWhiteSpace(SongSearchText))
        {
            FilteredSongsInPlaylist.Clear();
            foreach (var song in SongsInPlaylist)
            {
                FilteredSongsInPlaylist.Add(song);
            }
        }
        else
        {
            var filtered = SongsInPlaylist
                .Where(s => s.Name.Contains(SongSearchText, StringComparison.OrdinalIgnoreCase))
                .ToList();

            FilteredSongsInPlaylist.Clear();
            foreach (var song in filtered)
            {
                FilteredSongsInPlaylist.Add(song);
            }
        }
    }
        
    [RelayCommand]
    private async Task ShowLiveSearchModal()
    {
        try
        {
            var songsInPlaylistIds = SongsInPlaylist.Select(s => s.Id);
            var searchViewModel = new SearchSongsViewModel(_songFacade, Playlist.Id, songsInPlaylistIds);
        
            searchViewModel.SongSelected += async (sender, selectedSong) =>
            {
                await AddSelectedSongToPlaylist(selectedSong);
                await Application.Current.MainPage.Navigation.PopAsync(); 
            };
    
            searchViewModel.CloseRequested += async (sender, args) =>
            {
                await Application.Current.MainPage.Navigation.PopAsync(); 
            };
    
            var modalPage = new SearchSongsPage(searchViewModel); 
        
            // Zmena na normálnu navigáciu
            await Application.Current.MainPage.Navigation.PushAsync(modalPage);
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to open search: {ex.Message}", "OK");
        }
    }
}