using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ICS_Project.BL.Models;
using ICS_Project.BL.Models.Enums;

namespace ICSProject.MAUI.ViewModels;

public partial class PlaylistListViewModel : ObservableObject
{
    [ObservableProperty]
    private string _searchText = string.Empty;

    public ObservableCollection<PlaylistListModel> Playlists { get; } = new();

    public ObservableCollection<SortOptions> PlaylistSortOptions { get; } = new()
    {
        SortOptions.PlaylistName,
        SortOptions.PlaylistSongCount,
        SortOptions.PlaylistDuration
    };
}