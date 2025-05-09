using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICS_Project.BL.Facades;
using ICS_Project.BL.Models;

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
        
        try
        {
            var allSongs = await _songFacade.GetAsync();
            
            var songsInPlaylistIds = SongsInPlaylist.Select(s => s.Id).ToHashSet();
            var availableSongs = allSongs.Where(s => !songsInPlaylistIds.Contains(s.Id)).ToList();
            
            if (availableSongs.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Info", "No more songs available to add", "OK");
                return;
            }
            
            var songNames = availableSongs.Select(s => s.Name).ToArray();
            string result = await Application.Current.MainPage.DisplayActionSheet(
                "Add song to playlist", 
                "Cancel", 
                null, 
                songNames);
            
            if (result != "Cancel" && result != null)
            {
                var selectedSong = availableSongs.FirstOrDefault(s => s.Name == result);
                if (selectedSong != null)
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
                    
                    var updatedSongs = new List<PlaylistSongListModel>(Playlist.Songs);
                    updatedSongs.Add(playlistSong);
                    
                    Playlist = new PlaylistDetailModel
                    {
                        Id = Playlist.Id,
                        Name = Playlist.Name,
                        Description = Playlist.Description,
                        DurationInSeconds = Playlist.DurationInSeconds + songDetail.DurationInSeconds,
                        SongCount = Playlist.SongCount + 1,
                        Songs = new ObservableCollection<PlaylistSongListModel>(updatedSongs)
                    };
                    
                    SongsInPlaylist.Add(songDetail);
                    
                    var playlistToUpdate = new PlaylistDetailModel
                    {
                        Id = Playlist.Id,
                        Name = Playlist.Name,
                        Description = Playlist.Description,
                        DurationInSeconds = Playlist.DurationInSeconds + songDetail.DurationInSeconds,
                        SongCount = Playlist.SongCount + 1
                    };
                    
                    await _playlistFacade.SaveAsync(playlistToUpdate);
                    
                    OnPropertyChanged(nameof(Playlist));
                    OnPropertyChanged(nameof(SongsInPlaylist));
                    
                    await ReloadPlaylistAsync();
                    
                    PlaylistChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to add song: {ex.Message}", "OK");
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
    
    
    // [RelayCommand]
    // private async Task RemoveSongFromPlaylistAsync(SongDetailModel song)
    // {
    //     if (song == null) return;
    //
    //     bool confirm = await Application.Current.MainPage.DisplayAlert(
    //         "Remove Song", 
    //         $"Are you sure you want to remove '{song.Name}' from this playlist?", 
    //         "Yes", "No");
    //
    //     if (confirm)
    //     {
    //         try
    //         {
    //             // Find the playlist song
    //             var playlistSong = Playlist.Songs?.FirstOrDefault(ps => ps.SongId == song.Id);
    //             if (playlistSong != null)
    //             {
    //                 // Smazat vazbu pomocí PlaylistSongFacade
    //                 await _playlistSongFacade.DeleteAsync(playlistSong.Id);
    //                 
    //                 // Vytvoříme aktualizovanou kolekci Songs bez odebraného songu
    //                 var updatedSongs = new List<PlaylistSongListModel>(Playlist.Songs.Where(s => s.Id != playlistSong.Id));
    //                 
    //                 // Aktualizujeme Playlist s novou kolekcí
    //                 Playlist = new PlaylistDetailModel
    //                 {
    //                     Id = Playlist.Id,
    //                     Name = Playlist.Name,
    //                     Description = Playlist.Description,
    //                     DurationInSeconds = Playlist.DurationInSeconds,
    //                     SongCount = Playlist.SongCount - 1,
    //                     Songs = new ObservableCollection<PlaylistSongListModel>(updatedSongs)
    //                 };
    //                 
    //                 // Aktualizovat UI kolekci
    //                 SongsInPlaylist.Remove(song);
    //                 
    //                 // Aktualizovat playlist (bez Songs kolekce)
    //                 var playlistToUpdate = new PlaylistDetailModel
    //                 {
    //                     Id = Playlist.Id,
    //                     Name = Playlist.Name,
    //                     Description = Playlist.Description,
    //                     DurationInSeconds = Playlist.DurationInSeconds,
    //                     SongCount = Playlist.SongCount
    //                 };
    //                 
    //                 await _playlistFacade.SaveAsync(playlistToUpdate);
    //                 
    //                 PlaylistChanged?.Invoke(this, EventArgs.Empty);
    //             }
    //         }
    //         catch (Exception ex)
    //         {
    //             await Application.Current.MainPage.DisplayAlert("Error", $"Failed to remove song from playlist: {ex.Message}", "OK");
    //         }
    //     }
    // }
}