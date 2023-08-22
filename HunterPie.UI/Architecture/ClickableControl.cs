using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;

namespace HunterPie.UI.Architecture;

public class ClickableControl : UserControl
{
    private bool _isMouseInside;
    private bool _isMouseDown;

    public event EventHandler<EventArgs> OnClick;

    public static readonly RoutedEvent ClickEvent = RoutedEvent.Register<ClickableControl, RoutedEventArgs>(nameof(Click), RoutingStrategies.Bubble);

    public event EventHandler<RoutedEventArgs> Click
    {
        add => AddHandler(ClickEvent, value);
        remove => RemoveHandler(ClickEvent, value);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        _isMouseDown = true;
        e.Handled = true;
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        // Was a click!
        if (_isMouseDown && _isMouseInside)
        {
            OnClickEvent();
            OnClick?.Invoke(this, e);
            RaiseEvent(new RoutedEventArgs(ClickEvent, this));
        }

        _isMouseDown = false;
    }

    protected override void OnPointerEntered(PointerEventArgs e)
    {
        base.OnPointerEntered(e);

        _isMouseInside = true;

    }

    protected override void OnPointerExited(PointerEventArgs e)
    {
        base.OnPointerExited(e);

        _isMouseInside = false;
        _isMouseDown = false;
    }

    protected virtual void OnClickEvent()
    {

    }
}
