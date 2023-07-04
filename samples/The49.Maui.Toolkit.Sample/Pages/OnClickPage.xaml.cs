using System.Windows.Input;

namespace The49.Maui.Toolkit.Sample.Pages;

public partial class OnClickPage : ContentPage
{
	public OnClickPage()
	{
		InitializeComponent();
	}

	public ICommand ClickCommand => new Command(OnClick);

	void OnClick()
	{

	}
}