using System.Collections.ObjectModel;


namespace The49.Maui.Toolkit.Sample.Models;

public class CommentThread: ObservableCollection<Comment>
{
    public Comment Comment { get; set; }

}
