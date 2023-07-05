using System.Windows.Input;
using Maui.BindableProperty.Generator.Core;
using The49.Maui.Toolkit.Sample.Models;

namespace The49.Maui.Toolkit.Sample.Views;

public partial class CommentCard
{
    [AutoBindable]
    readonly Comment _comment;

    [AutoBindable]
    readonly ICommand _command;

    [AutoBindable]
    readonly object _commandParameter;

    public CommentCard()
	{
		InitializeComponent();
	}
}