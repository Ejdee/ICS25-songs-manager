using CommunityToolkit.Maui.Views;

namespace ICSProject.MAUI.Views;

public partial class AddSongPopup : Popup
{
    private new TaskCompletionSource<(string Name, string Author, string Genre, string SongUrl, string Duration)> Result { get; } = new();

    public AddSongPopup()
    {
        InitializeComponent();
    }

    private void OnSaveClicked(object sender, EventArgs e)
    {
        Result.SetResult((NameEntry.Text, AuthorEntry.Text, GenreEntry.Text, SongUrlEntry.Text, DurationEntry.Text));
        Close((NameEntry.Text,AuthorEntry.Text, GenreEntry.Text, SongUrlEntry.Text, DurationEntry.Text));
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        Result.SetResult((null, null, null, null, null)!);
        Close();
    }
}