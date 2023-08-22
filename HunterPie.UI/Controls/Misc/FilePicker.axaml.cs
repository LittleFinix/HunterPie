using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using HunterPie.Core.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.UI.Controls.Misc;

[TemplatePart("PART_OpenDialog", typeof(Button))]
[PseudoClasses(PSEUDO_CLASS_ALLOW_MULTIPLE, PSEUDO_CLASS_MULTIPLE)]
public class FilePicker : TemplatedControl
{
    public const string PSEUDO_CLASS_EMPTY = ":empty";
    public const string PSEUDO_CLASS_SINGLE = ":single";
    public const string PSEUDO_CLASS_MULTIPLE = ":multiple";
    public const string PSEUDO_CLASS_ALLOW_MULTIPLE = ":allow-multiple";

    public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<FilePicker, string>(
        "Title");

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    
    public static readonly StyledProperty<IReadOnlyList<FilePickerFileType>> FileFilterProperty = AvaloniaProperty.Register<FilePicker, IReadOnlyList<FilePickerFileType>>(
        "FileFilter");

    public IReadOnlyList<FilePickerFileType> FileFilter
    {
        get => GetValue(FileFilterProperty);
        set => SetValue(FileFilterProperty, value);
    }
    
    public static readonly StyledProperty<bool> AllowMultipleProperty = AvaloniaProperty.Register<FilePicker, bool>(
        "AllowMultiple");

    public bool AllowMultiple
    {
        get => GetValue(AllowMultipleProperty);
        set => SetValue(AllowMultipleProperty, value);
    }

    public static readonly StyledProperty<string?> SingleFileProperty = AvaloniaProperty.Register<FilePicker, string?>(
        "SingleFile");

    public string? SingleFile
    {
        get => GetValue(SingleFileProperty);
        set => SetValue(SingleFileProperty, value);
    }

    public static readonly StyledProperty<IList<string>?> FilesProperty = AvaloniaProperty.Register<FilePicker, IList<string>?>(
        "Files");

    public IList<string>? Files
    {
        get => GetValue(FilesProperty);
        set => SetValue(FilesProperty, value);
    }

    protected Button? PART_OpenDialog;

    private IStorageProvider? Provider => TopLevel.GetTopLevel(this)?.StorageProvider;

    protected virtual FilePickerOpenOptions Options => new()
    {
        Title = Title,
        AllowMultiple = AllowMultiple,
        FileTypeFilter = FileFilter,
    };

    protected override void OnInitialized()
    {
        base.OnInitialized();
        PseudoClasses.Set(PSEUDO_CLASS_ALLOW_MULTIPLE, this.GetObservable(AllowMultipleProperty));
        PseudoClasses.Set(PSEUDO_CLASS_SINGLE,
            this.GetObservable(FilesProperty, f => f?.Count == 1)
        );
        PseudoClasses.Set(PSEUDO_CLASS_EMPTY,
            this.GetObservable(FilesProperty, f => f is null || f.Count == 0)
        );
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        if (PART_OpenDialog is not null)
            PART_OpenDialog.Click -= PART_OpenDialogOnClick;
        
        PART_OpenDialog = e.NameScope.Get<Button>("PART_OpenDialog");
        
        PART_OpenDialog.Click += PART_OpenDialogOnClick;
        PART_OpenDialog.IsEnabled = Provider?.CanOpen == true;
    }

    private async void PART_OpenDialogOnClick(object? sender, RoutedEventArgs e)
    {
        var files = await Provider!.OpenFilePickerAsync(Options);
        
        Files = files.Select(f => f.TryGetLocalPath()).FilterNull().ToArray();

        if (!AllowMultiple)
            SingleFile = Files.FirstOrDefault();

        PseudoClasses.Set(PSEUDO_CLASS_MULTIPLE, AllowMultiple && Files.Count > 1);
    }
}