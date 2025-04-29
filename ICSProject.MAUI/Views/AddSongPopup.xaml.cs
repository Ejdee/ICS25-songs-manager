using CommunityToolkit.Maui.Views;

namespace ICSProject.MAUI.Views;

public partial class AddSongPopup : Popup
{
    public TaskCompletionSource<(string Name, string Genre, string Duration)> Result { get; } = new();

    public AddSongPopup()
    {
        InitializeComponent();
    }

    private void OnSaveClicked(object sender, EventArgs e)
    {
        Result.SetResult((NameEntry.Text, GenreEntry.Text, DurationEntry.Text));
        Close((NameEntry.Text, GenreEntry.Text, DurationEntry.Text));
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        Result.SetResult((null, null, null));
        Close();
    }
}