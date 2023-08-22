using Avalonia.Controls;

namespace HunterPie.UI.Controls.Buttons;

/// <summary>
/// Interaction logic for ColorPicker.xaml
/// </summary>
public partial class ColorPicker : UserControl
{
    public ColorPicker()
    {
        InitializeComponent();
    }

    private void PickColor()
    {
        // using ColorDialog colorDialog = new();
        //
        // if (colorDialog.ShowDialog() != NativeDialogResult.OK)
        //     return;
        //
        // if (DataContext is Color color)
        //     color.Value = Avalonia.Media.Color.ToHexString();
    }
}
