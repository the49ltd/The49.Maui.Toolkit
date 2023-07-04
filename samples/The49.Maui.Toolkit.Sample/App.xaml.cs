using The49.Maui.Toolkit.Sample.Pages;

namespace The49.Maui.Toolkit.Sample;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(OnClickPage), typeof(OnClickPage));

        MainPage = new AppShell();
    }
}
