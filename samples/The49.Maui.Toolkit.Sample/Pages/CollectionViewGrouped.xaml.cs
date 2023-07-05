using System.Collections.ObjectModel;
using The49.Maui.Toolkit.Sample.Models;

namespace The49.Maui.Toolkit.Sample.Pages;

public partial class CollectionViewGrouped : ContentPage
{
    ObservableCollection<CommentThread> _comments;

    public ObservableCollection<CommentThread> Comments => _comments;
    public CollectionViewGrouped()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        var t1 = new CommentThread
        {
            Comment = new Comment { UserName = "@NuthinButNet", Text = "Commenting", AvatarUrl = "user.png" },
        };

        var t2 = new CommentThread
        {
            new Comment { UserName = "@NuthinButNet", Text = "Commenting", AvatarUrl = "user.png" },
            new Comment { UserName = "@NuthinButNet", Text = "Commenting", AvatarUrl = "user.png" },
            new Comment { UserName = "@NuthinButNet", Text = "Commenting", AvatarUrl = "user.png" },
            new Comment { UserName = "@NuthinButNet", Text = "Commenting", AvatarUrl = "user.png" },
            new Comment { UserName = "@NuthinButNet", Text = "Commenting", AvatarUrl = "user.png" },
            new Comment { UserName = "@NuthinButNet", Text = "Commenting", AvatarUrl = "user.png" },
        };

        t2.Comment = new Comment { UserName = "@NuthinButNet", Text = "Commenting", AvatarUrl = "user.png" };

        _comments = new ObservableCollection<CommentThread>
        {
            t1,
            t2,
        };

        OnPropertyChanged(nameof(Comments));
    }
}