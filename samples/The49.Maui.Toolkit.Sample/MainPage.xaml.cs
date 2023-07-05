using The49.Maui.Toolkit.Sample.Pages;

namespace The49.Maui.Toolkit.Sample;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(OnClickPage));
    }
}

