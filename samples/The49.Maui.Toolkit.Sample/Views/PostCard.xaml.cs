using Maui.BindableProperty.Generator.Core;

namespace The49.Maui.Toolkit.Sample.Views;

public partial class PostCard
{
    [AutoBindable]
    readonly string _avatarUrl;

    [AutoBindable]
	readonly string _userName;

    [AutoBindable]
    readonly string _text;
    public PostCard()
	{
		InitializeComponent();
	}
}