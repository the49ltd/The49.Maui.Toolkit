﻿using Foundation;
using Microsoft.Maui.Controls.Handlers.Items;

namespace The49.Maui.Toolkit.Handlers;

class EmptySource : ILoopItemsViewSource
{
    public int GroupCount => 0;

    public int ItemCount => 0;

    public bool Loop { get; set; }

    public int LoopCount => 0;

    public object this[NSIndexPath indexPath] => throw new IndexOutOfRangeException("IItemsViewSource is empty");

    public int ItemCountInGroup(nint group) => 0;

    public object Group(NSIndexPath indexPath)
    {
        throw new IndexOutOfRangeException("IItemsViewSource is empty");
    }

    public IItemsViewSource GroupItemsViewSource(NSIndexPath indexPath)
    {
        throw new IndexOutOfRangeException("IItemsViewSource is empty");
    }

    public NSIndexPath GetIndexForItem(object item)
    {
        throw new IndexOutOfRangeException("IItemsViewSource is empty");
    }

    public void Dispose()
    {
    }
}