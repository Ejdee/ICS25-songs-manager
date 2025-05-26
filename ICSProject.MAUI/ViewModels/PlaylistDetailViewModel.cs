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
    
    [ObservableProperty]
    private bool _hasCustomImage;
    
    public event EventHandler? SaveCompleted;
    public event EventHandler? PlaylistChanged;
    public event EventHandler<PlaylistDetailModel>? EditPlaylistRequested;
    public event EventHandler? PlaylistDeleted;
    
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
                ImageUrl = Playlist.ImageUrl,
                DurationInSeconds = Playlist.DurationInSeconds,
                SongCount = Playlist.SongCount
            };
            
            await _playlistFacade.SaveAsync(playlistToSave);
            PlaylistChanged?.Invoke(this, EventArgs.Empty);
            SaveCompleted?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to save playlist: {ex.Message}", "OK");
            }
            else
            {
                Console.WriteLine($"Failed to save playlist: {ex.Message}");
            }
        }
    }
    
    [RelayCommand]
    private async Task DeleteAsync()
    {
        try
        {
            var mainPage = Application.Current?.MainPage;
            if (mainPage == null) return;

            bool confirm = await mainPage.DisplayAlert(
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
                    PlaylistDeleted?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    if (Application.Current?.MainPage != null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", $"Failed to delete playlist: {ex.Message}", "OK");
                    }
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
        await ShowSongSearchAsync();
    }

    [RelayCommand]
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
        var mainPage = Application.Current?.MainPage;
        if (mainPage == null) return;

        bool confirm = await mainPage.DisplayAlert(
            "Remove Song", 
            $"Are you sure you want to remove '{song.Name}' from this playlist?", 
            "Yes", "No");

        if (confirm)
        {
            try
            {
                var playlistSong = Playlist.Songs?.FirstOrDefault(ps => ps.SongId == song.Id);
                if (playlistSong != null)
                {
                    await _playlistSongFacade.DeleteAsync(playlistSong.Id);
                    await ReloadPlaylistAsync();
                    PlaylistChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"Failed to remove song from playlist: {ex.Message}", "OK");
                }
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
    private void NavigateToEdit()
    {
        EditPlaylistRequested?.Invoke(this, Playlist);
    }
    
    [RelayCommand]
    private async Task ShowSongSearchAsync()
    {
        try
        {
            var songsInPlaylistIds = SongsInPlaylist.Select(s => s.Id);
            var searchViewModel = new SearchSongsViewModel(_songFacade, Playlist.Id, songsInPlaylistIds);
            
            searchViewModel.MultipleSongsSelected += async (sender, selectedSongs) =>
            {
                await AddMultipleSongsToPlaylist(selectedSongs);
                if (Application.Current?.MainPage?.Navigation != null)
                {
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
            };

            searchViewModel.CloseRequested += async (sender, args) =>
            {
                if (Application.Current?.MainPage?.Navigation != null)
                {
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
            };

            var modalPage = new SearchSongsPage(searchViewModel); 
    
            if (Application.Current?.MainPage?.Navigation != null)
            {
                await Application.Current.MainPage.Navigation.PushAsync(modalPage);
            }
        }
        catch (Exception ex)
        {
            await ShowErrorAlert($"Failed to open search: {ex.Message}");
        }
    }
    
    private async Task AddMultipleSongsToPlaylist(List<SongListModel> selectedSongs)
    {
        try
        {
            foreach (var selectedSong in selectedSongs)
            {
                var songDetail = await _songFacade.GetAsync(selectedSong.Id);
                if (songDetail == null) continue;

                var playlistSong = new PlaylistSongListModel
                {
                    Id = Guid.NewGuid(),
                    SongId = songDetail.Id,
                    SongName = songDetail.Name,
                    SongDurationInSeconds = songDetail.DurationInSeconds
                };
            
                await _playlistSongFacade.SaveAsync(playlistSong, Playlist.Id);
            }
        
            await ReloadPlaylistAsync();
            PlaylistChanged?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            await ShowErrorAlert($"Failed to add songs to playlist: {ex.Message}");
        }
    }
    
    partial void OnPlaylistChanged(PlaylistDetailModel value)
    {
        HasCustomImage = !string.IsNullOrEmpty(value.ImageUrl);
    }
    
    [RelayCommand]
    private async Task ManageImageAsync()
    {
        var mainPage = Application.Current?.MainPage;
        if (mainPage == null) return;

        var options = new List<string>();

        if (string.IsNullOrEmpty(Playlist.ImageUrl))
        {
            options.Add("Enter Image URL");
        }
        else
        {
            options.Add("Edit Image URL");
            options.Add("Remove Image");
        }

        var action = await mainPage.DisplayActionSheet(
            "Playlist Image", 
            "Cancel", 
            null, 
            options.ToArray());

        switch (action)
        {
            case "Enter Image URL":
            case "Edit Image URL":
                await ChangeImageUrlAsync();
                break;
            case "Remove Image":
                await RemoveImageAsync();
                break;
        }
    }
    
    [RelayCommand]
    private async Task ChangeImageUrlAsync()
    {
        try
        {
            var mainPage = Application.Current?.MainPage;
            if (mainPage == null) return;

            var result = await mainPage.DisplayPromptAsync(
                "Image URL", 
                "Enter image URL:", 
                "OK", 
                "Cancel", 
                "https://example.com/image.jpg",
                -1,
                Keyboard.Url,
                Playlist.ImageUrl ?? string.Empty);

            if (!string.IsNullOrWhiteSpace(result))
            {
                if (IsValidImageUrl(result))
                {
                    Playlist = Playlist with { ImageUrl = result };
                    await SaveImageOnlyAsync();
                }
                else
                {
                    await ShowErrorAlert("Please enter a valid image URL");
                }
            }
        }
        catch (Exception ex)
        {
            await ShowErrorAlert($"Failed to change image: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task RemoveImageAsync()
    {
        var mainPage = Application.Current?.MainPage;
        if (mainPage == null) return;

        bool confirm = await mainPage.DisplayAlert(
            "Remove Image", 
            "Are you sure you want to remove the playlist image?", 
            "Yes", "No");

        if (confirm)
        {
            Playlist = Playlist with { ImageUrl = string.Empty };
            await SaveImageOnlyAsync();
        }
    }

    private async Task ShowErrorAlert(string message)
    {
        if (Application.Current?.MainPage != null)
        {
            await Application.Current.MainPage.DisplayAlert("Error", message, "OK");
        }
        else
        {
            Console.WriteLine(message);
        }
    }
    
    private bool IsValidImageUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
    
    private async Task SaveImageOnlyAsync()
    {
        try
        { 
            var playlistToSave = new PlaylistDetailModel
            {
                Id = Playlist.Id,
                Name = Playlist.Name,
                Description = Playlist.Description,
                ImageUrl = Playlist.ImageUrl,
                DurationInSeconds = Playlist.DurationInSeconds,
                SongCount = Playlist.SongCount
            };
        
            await _playlistFacade.SaveAsync(playlistToSave);
        }
        catch (Exception ex)
        {
            await ShowErrorAlert($"Failed to save playlist: {ex.Message}");
        }
    }
}