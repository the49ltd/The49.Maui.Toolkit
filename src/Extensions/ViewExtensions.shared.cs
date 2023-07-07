namespace The49.Maui.Toolkit.Extensions;

public static class ViewExtensions
{
    public static Task EnsureHandlerAsync(this View view)
    {
        if (view.Handler is not null)
        {
            return Task.CompletedTask;
        }

        var tcs = new TaskCompletionSource();

        void OnHandlerChanged(object sender, EventArgs e)
        {
            if (sender is View v && v.Handler is not null)
            {
                tcs.SetResult();
                view.HandlerChanged -= OnHandlerChanged;
            }
        }

        view.HandlerChanged += OnHandlerChanged;

        return tcs.Task;
    }
}
