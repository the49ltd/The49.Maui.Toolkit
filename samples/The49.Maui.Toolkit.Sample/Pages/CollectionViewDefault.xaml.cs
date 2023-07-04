using The49.Maui.Toolkit.Sample.Models;

namespace The49.Maui.Toolkit.Sample.Pages;

public partial class CollectionViewDefault : ContentPage
{
    IEnumerable<SocialPost> _posts;

    public IEnumerable<SocialPost> Posts => _posts;

    public CollectionViewDefault()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _posts = SocialPost.GetSample();
        OnPropertyChanged(nameof(Posts));
    }
}