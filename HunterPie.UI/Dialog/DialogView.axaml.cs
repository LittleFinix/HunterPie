using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using Avalonia.Styling;
using HunterPie.Core.Domain.Dialog;
using System.ComponentModel;
using System.Threading.Tasks;

namespace HunterPie.UI.Dialog;

/// <summary>
/// Interaction logic for DialogView.xaml
/// </summary>
public partial class DialogView : Window, INativeDialog, INotifyPropertyChanged
{
    public NativeDialogResult DialogResult { get; private set; }

    public static readonly StyledProperty<NativeDialogButtons> ButtonsProperty = AvaloniaProperty.Register<DialogView, NativeDialogButtons>(
        "Buttons");

    public NativeDialogButtons Buttons
    {
        get => GetValue(ButtonsProperty);
        set => SetValue(ButtonsProperty, value);
    }

    public static readonly StyledProperty<string> DialogTitleProperty = AvaloniaProperty.Register<DialogView, string>(
        "DialogTitle");

    public string DialogTitle
    {
        get => GetValue(DialogTitleProperty);
        set => SetValue(DialogTitleProperty, value);
    }

    public static readonly StyledProperty<string> DescriptionProperty = AvaloniaProperty.Register<DialogView, string>(
        "Description", "This is a default dialog text");

    public string Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public DialogView()
    {
        InitializeComponent();
    }

    public void Warn(string title, string description, NativeDialogButtons buttons) => SetDialogInfo(title, description, "ICON_WARN", buttons);

    public void Info(string title, string description, NativeDialogButtons buttons) => SetDialogInfo(title, description, "ICON_INFO", buttons);

    public void Error(string title, string description, NativeDialogButtons buttons) => SetDialogInfo(title, description, "ICON_ERROR", buttons);

    private TaskCompletionSource<NativeDialogResult>? result;
    
    private void SetDialogInfo(string title, string description, string icon, NativeDialogButtons buttons)
    {
        DialogTitle = title;
        Description = description;
        
        if (TryGetResource(icon, ThemeVariant.Default, out object? bmp))
            Icon = new WindowIcon((Bitmap)bmp);

        Buttons = buttons;

        result?.SetCanceled();
        result = new();
            
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop &&
            desktop.MainWindow is not null)
            ShowDialog(desktop.MainWindow);
        else
            Show();
    }

    Task<NativeDialogResult> INativeDialog.DialogResult() => result?.Task ?? Task.FromResult(NativeDialogResult.NotFinished);

    public void OnAccept()
    {
        DialogResult = NativeDialogResult.Accept;
        Close();
    }

    public void OnReject()
    {
        DialogResult = NativeDialogResult.Reject;
        Close();
    }

    public void OnCancel()
    {
        DialogResult = NativeDialogResult.Cancel;
        Close();
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        base.OnClosing(e);
        
        if (DialogResult == NativeDialogResult.NotFinished)
            DialogResult = NativeDialogResult.Cancel;
        
        result?.SetResult(DialogResult);
    }
}
