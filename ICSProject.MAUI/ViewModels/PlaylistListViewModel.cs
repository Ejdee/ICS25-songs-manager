﻿using System.Collections.ObjectModel;
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
    
    [ObservableProperty]
    private bool _isSortAscending = true;

    public IAsyncRelayCommand LoadPlaylistsCommand { get; }
    public IRelayCommand AddPlaylistPopupCommand { get; }
    public IAsyncRelayCommand<PlaylistListModel> ShowPlaylistCommand { get; }
    public IAsyncRelayCommand<PlaylistListModel> EditPlaylistCommand { get; }

    public event EventHandler? AddPlaylistRequested;
    public event EventHandler<PlaylistDetailModel>? NavigateToDetailRequested;
    public event EventHandler<PlaylistDetailModel>? NavigateToPlaylistSongsRequested;

    public PlaylistListViewModel(PlaylistFacade playlistFacade)
    {
        _playlistFacade = playlistFacade;

        LoadPlaylistsCommand = new AsyncRelayCommand(LoadPlaylistsAsync);
        AddPlaylistPopupCommand = new RelayCommand(() => AddPlaylistRequested?.Invoke(this, EventArgs.Empty));
        EditPlaylistCommand = new AsyncRelayCommand<PlaylistListModel>(NavigateToEditPage);
        ShowPlaylistCommand = new AsyncRelayCommand<PlaylistListModel>(NavigateToPlaylistSongsPage);
        
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
                playlists = await _playlistFacade.GetSortedAsync(SelectedSortOption);
            }
            
            playlists = (SelectedSortOption, IsSortAscending) switch
            {
                (SortOptions.PlaylistName, true) => playlists.OrderBy(p => p.Name, StringComparer.OrdinalIgnoreCase),
                (SortOptions.PlaylistName, false) => playlists.OrderByDescending(p => p.Name, StringComparer.OrdinalIgnoreCase),
                (SortOptions.PlaylistSongCount, true) => playlists.OrderBy(p => p.SongCount),
                (SortOptions.PlaylistSongCount, false) => playlists.OrderByDescending(p => p.SongCount),
                (SortOptions.PlaylistDuration, true) => playlists.OrderBy(p => p.DurationInSeconds),
                (SortOptions.PlaylistDuration, false) => playlists.OrderByDescending(p => p.DurationInSeconds),
                _ => playlists
            };
            
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

    public async Task AddPlaylistAsync(string name, string description, string? imageUrl)
    {
        var newPlaylist = new PlaylistDetailModel
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description ?? string.Empty,
            ImageUrl = imageUrl ?? "song_placeholder.png",
            DurationInSeconds = TimeSpan.Zero,
            SongCount = 0
        };

        await _playlistFacade.SaveAsync(newPlaylist);
        await LoadPlaylistsAsync();
    }
    
    [RelayCommand]
    private async Task ToggleSortDirection()
    {
        IsSortAscending = !IsSortAscending;
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

    private async Task NavigateToPlaylistSongsPage(PlaylistListModel? playlist)
    {
        if (playlist != null)
        {
            try
            {
                var detail = await _playlistFacade.GetAsync(playlist.Id);
                if (detail != null)
                {
                    NavigateToPlaylistSongsRequested?.Invoke(this, detail);
                }
            } catch (Exception e)
            {
                Console.WriteLine($"ERROR - navigating to playlist songs: {e.Message}");
            }
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