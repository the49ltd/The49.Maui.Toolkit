namespace The49.Maui.Toolkit;

public class Mention
{
    public int Index { get; set; }
    public string Text { get; set; }
    public object Data { get; set; }
}

public class MentionsBehavior : Behavior<View>
{
    public static readonly BindableProperty SpanTemplateProperty = BindableProperty.Create(nameof(SpanTemplate), typeof(DataTemplate), typeof(MentionsBehavior));
    public static readonly BindableProperty MentionSpanTemplateProperty = BindableProperty.Create(nameof(MentionSpanTemplate), typeof(DataTemplate), typeof(MentionsBehavior));

    public DataTemplate SpanTemplate
    {
        get => (DataTemplate)GetValue(SpanTemplateProperty);
        set => SetValue(SpanTemplateProperty, value);
    }

    public DataTemplate MentionSpanTemplate
    {
        get => (DataTemplate)GetValue(MentionSpanTemplateProperty);
        set => SetValue(MentionSpanTemplateProperty, value);
    }

    ITextInput _textInput;

    List<Mention> _mentions = new List<Mention>
    {
        new Mention { Index = 0, Text = "@NuthinButNet", Data = "@NuthinButNet" },
    };

    protected override void OnAttachedTo(BindableObject bindable)
    {
        if (bindable is not ITextInput ti)
        {
            throw new Exception("Can only use mentions on a ITextInput");
        }
        _textInput = ti;
        SetupTextInput();
        UpdateFormattedText();

        base.OnAttachedTo(bindable);
    }

    protected override void OnDetachingFrom(BindableObject bindable)
    {
        base.OnDetachingFrom(bindable);
        TeardownTextInput();
    }

    void SetupTextInput()
    {
        if (_textInput is Entry ti)
        {
            ti.TextChanged += OnTextChanged;
        }
    }

    void TeardownTextInput()
    {
        if (_textInput is null)
        {
            return;
        }

        if (_textInput is Entry entry)
        {
            entry.TextChanged -= OnTextChanged;
        }
    }

    void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (e.OldTextValue == e.NewTextValue)
        {
            return;
        }
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var index = ((ITextInput)sender).CursorPosition;

            var before = e.OldTextValue is null ? 0 : e.OldTextValue.Length;
            var count = e.NewTextValue is null ? 0 : e.NewTextValue.Length;

            UpdateMentionsHighlight(index, before, count);
        });
    }

    void UpdateMentionsHighlight(int index, int before, int count)
    {
        if (_mentions.Count == 0)
        {
            return;
        }

        for (var i = _mentions.Count - 1; i >= 0; i--)
        {
            var mention = _mentions[i];
            int mentionStart = mention.Index;
            int mentionEnd = mentionStart + mention.Text.Length;
            int editPos = index + (count - before);

            if (index <= mentionStart)
            {
                //Editing text before mention - change offset
                int diff = count - before;
                mention.Index = mentionStart + diff;
            }
            else if (editPos > mentionStart + 1 && editPos < mentionEnd)
            {
                //Editing text within mention - delete the mention
                _mentions.RemoveAt(i);
            }
        }
        UpdateFormattedText();

    }

    Span GetTextSpan(string text)
    {
        if (SpanTemplate is null)
        {
            return new Span { Text = text };
        }

        var s = SpanTemplate.CreateContent();

        if (s is not Span span)
        {
            throw new Exception("SpanTemplate is not creating a Span");
        }

        span.BindingContext = text;

        return span;
    }

    Span GetMentionSpan(Mention mention)
    {
        if (MentionSpanTemplate is null)
        {
            return new Span { Text = mention.Text };
        }

        var s = MentionSpanTemplate.CreateContent();

        if (s is not Span span)
        {
            throw new Exception("MentionSpanTemplate is not creating a Span");
        }

        span.BindingContext = mention.Data;

        return span;
    }

    void UpdateFormattedText()
    {
        var mentions = _mentions.OrderBy(m => m.Index);

        var cursor = 0;

        var fs = new FormattedString();

        string section;

        foreach (var mention in mentions)
        {
            section = _textInput.Text.Substring(cursor, mention.Index - cursor);
            if (section.Length != 0)
            {
                fs.Spans.Add(GetTextSpan(section));
            }
            section = _textInput.Text.Substring(mention.Index, mention.Text.Length);
            fs.Spans.Add(GetMentionSpan(mention));
            cursor = mention.Index + mention.Text.Length;
        }
        section = _textInput.Text.Substring(cursor, _textInput.Text.Length - cursor);
        if (section.Length != 0)
        {
            fs.Spans.Add(GetTextSpan(section));
        }

        if (_textInput is View view)
        {
            FormattedText.SetFormattedText(view, fs);
        }
    }
}
