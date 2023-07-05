using Maui.BindableProperty.Generator.Core;
using The49.Maui.Toolkit.Sample.Models;

namespace The49.Maui.Toolkit.Sample.Views;

public partial class CommentCard
{
    [AutoBindable]
    readonly Comment _comment;

    public CommentCard()
	{
		InitializeComponent();
	}
}