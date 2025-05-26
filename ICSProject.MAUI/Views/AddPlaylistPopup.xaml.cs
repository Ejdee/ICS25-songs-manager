using CommunityToolkit.Maui.Views;

namespace ICSProject.MAUI.Views;

public partial class AddPlaylistPopup : Popup
{
    public AddPlaylistPopup()
    {
        InitializeComponent();
    }

    private void OnSaveClicked(object sender, EventArgs e)
    {
        Close((NameEntry.Text, DescriptionEntry.Text, ImageUrlEntry.Text));
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close();
    }
}