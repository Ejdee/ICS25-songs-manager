using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICS_Project.BL.Models;
using ICS_Project.BL.Models.Enums;
using ICSProject.MAUI.Enums;

namespace ICSProject.MAUI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public bool ShowSortOptions => ContentType != ContentType.PlaylistSongs; 
    
    public SongListViewModel SongListViewModel { get; }
    public PlaylistListViewModel PlaylistListViewModel { get; }
    public PlaylistDetailViewModel PlaylistDetailViewModel { get; }
    public string CurrentTitle => ContentType switch
    {
        ContentType.Songs => "Songs",
        ContentType.Playlists => "Playlists",
        ContentType.PlaylistSongs => PlaylistDetailViewModel.Playlist.Name,
        _ => "Songs"
    };

    public bool ShowGenreFilter => ContentType == ContentType.Songs;
    public ObservableCollection<SortOptions> CurrentSortOptions =>
        ContentType == ContentType.Songs
            ? SongListViewModel.SongSortOptions
            : PlaylistListViewModel.PlaylistSortOptions;

    [ObservableProperty] 
    private object _currentViewModel;
    
    [ObservableProperty] 
    private ContentType _contentType;

    [ObservableProperty] 
    private SortOptions _currentSelectedSortOption;
    
    [RelayCommand]
    private void SetView(string viewType)
    {
        if (viewType == "Songs")
        {
            ContentType = ContentType.Songs;
            CurrentViewModel = SongListViewModel;
            _ = SongListViewModel.LoadSongsAsync();
        }
        else if (viewType == "Playlists")
        {
            ContentType = ContentType.Playlists;
            CurrentViewModel = PlaylistListViewModel;
        } else if (viewType == "Playlist Songs")
        {
            ContentType = ContentType.PlaylistSongs;
            CurrentViewModel = PlaylistDetailViewModel;
        }
    }
    
    public MainViewModel(SongListViewModel songListViewModel, PlaylistListViewModel playlistListViewModel, PlaylistDetailViewModel playlistDetailViewModel)
    {
        SongListViewModel = songListViewModel;
        PlaylistListViewModel = playlistListViewModel;
        PlaylistDetailViewModel = playlistDetailViewModel;

        _currentViewModel = SongListViewModel;
        _contentType = ContentType.Songs;
    }

    [RelayCommand]
    private async Task ShowSongs()
    {
        ContentType = ContentType.Songs;
        CurrentViewModel = SongListViewModel;
        await SongListViewModel.LoadSongsAsync();
    }

    [RelayCommand]
    private void ShowPlaylists()
    {
        ContentType = ContentType.Playlists;
        CurrentViewModel = PlaylistListViewModel;
    }
    
    [RelayCommand]
    private async Task ShowPlaylistSongs(PlaylistDetailModel playlist)
    {
        await PlaylistDetailViewModel.Load(playlist);
        ContentType = ContentType.PlaylistSongs;
        CurrentViewModel = PlaylistDetailViewModel;
    }

    [RelayCommand]
    private void SwitchView()
    {
        if (ContentType == ContentType.Songs)
        {
            ShowPlaylists();
            CurrentViewModel = PlaylistListViewModel;
        }
        else
        {
            _ = ShowSongs();
            CurrentViewModel = SongListViewModel;
        }
    }

    partial void OnContentTypeChanged(ContentType value)
    {
        OnPropertyChanged(nameof(CurrentTitle));
        OnPropertyChanged(nameof(ShowGenreFilter));
        OnPropertyChanged(nameof(CurrentSortOptions));
        OnPropertyChanged(nameof(ShowSortOptions));
    }

    partial void OnCurrentSelectedSortOptionChanged(SortOptions value)
    {
        if (ContentType == ContentType.Songs)
        {
            SongListViewModel.SelectedSortOption = value;
        }
        else
        {
            PlaylistListViewModel.SelectedSortOption = value;
        }
    }
}