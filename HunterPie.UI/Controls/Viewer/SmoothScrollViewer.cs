using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using System;
using System.Collections.Generic;

namespace HunterPie.UI.Controls.Viewer;
public class SmoothScrollViewer : ScrollViewer
{
    private double _totalVerticalOffset;
    private readonly Queue<double> _verticalOffsetQueue = new();
    private bool _isScrolling;

    public double CurrentVerticalOffset
    {
        get => GetValue(CurrentVerticalOffsetProperty);
        set => SetValue(CurrentVerticalOffsetProperty, value);
    }

    // Using a DependencyProperty as the backing store for CurrentVerticalOffset.  This enables animation, styling, binding, etc...
    public static readonly StyledProperty<double> CurrentVerticalOffsetProperty =
        AvaloniaProperty.Register<SmoothScrollViewer, double>(nameof(CurrentVerticalOffset));

    protected override void OnScrollChanged(ScrollChangedEventArgs e)
    {
        e.Handled = true;
        
        if (!_isScrolling)
        {
            _totalVerticalOffset = Offset.Y;
            CurrentVerticalOffset = Offset.Y;
        }

        double x = _totalVerticalOffset - (e.OffsetDelta.Y / 2);
        _totalVerticalOffset = Math.Min(Math.Max(0, x), ScrollBarMaximum.Y);
        AnimateVerticalScrolling(_totalVerticalOffset);
    }

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        e.Handled = true;

        if (!_isScrolling)
        {
            _totalVerticalOffset = Offset.Y;
            CurrentVerticalOffset = Offset.Y;
        }

        double x = _totalVerticalOffset - (e.Delta.Y / 2);
        _totalVerticalOffset = Math.Min(Math.Max(0, x), ScrollBarMaximum.Y);
        AnimateVerticalScrolling(_totalVerticalOffset);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        if (change.Property == CurrentVerticalOffsetProperty && change.NewValue is double value)
        {
            Offset = Offset.WithY(value);
        }
        
        base.OnPropertyChanged(change);
    }

    private void AnimateVerticalScrolling(double offset, double duration = 200)
    {
        // _verticalOffsetQueue.Enqueue(offset);
        // var animation = new DoubleAnimation(offset, TimeSpan.FromMilliseconds(duration))
        // {
        //     EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
        //     FillBehavior = FillBehavior.Stop
        // };
        // animation.Completed += OnAnimationCompleted;
        // _isScrolling = true;
        //
        // BeginAnimation(CurrentVerticalOffsetProperty, animation, HandoffBehavior.Compose);
    }

    private void OnAnimationCompleted(object sender, EventArgs e)
    {
        // if (sender is Timeline tl)
        //     tl.Completed -= OnAnimationCompleted;
        //
        // CurrentVerticalOffset = _verticalOffsetQueue.Dequeue();
        // _isScrolling = false;
    }
}
