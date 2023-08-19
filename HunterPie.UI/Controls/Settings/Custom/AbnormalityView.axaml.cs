using Avalonia.Controls;
using HunterPie.UI.Controls.Settings.Custom.Abnormality;
using System;

namespace HunterPie.UI.Controls.Settings.Custom;

/// <summary>
/// Interaction logic for AbnormalityView.xaml
/// </summary>
public partial class AbnormalityView : UserControl
{
    public AbnormalityView()
    {
        InitializeComponent();
    }

    private void OnClick()
    {
        if (DataContext is AbnormalityViewModel vm)
        {
            vm.IsEnabled = !vm.IsEnabled;
        }
    }
}
