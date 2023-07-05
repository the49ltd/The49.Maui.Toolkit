using System.Collections.ObjectModel;


namespace The49.Maui.Toolkit.Sample.Models;

public class CommentThread: ObservableCollection<Comment>
{
    private IEnumerable<Comment> _comments;

    public CommentThread(IEnumerable<Comment> comments): base()
    {
        _comments = comments;
    }
    public Comment Comment { get; set; }

    public void ShowReplies()
    {
        foreach (var reply in _comments)
        {
            Add(reply);
        }
    }

}
