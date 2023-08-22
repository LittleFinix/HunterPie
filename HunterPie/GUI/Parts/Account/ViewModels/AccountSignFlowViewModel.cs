using HunterPie.UI.Architecture;

namespace HunterPie.GUI.Parts.Account.ViewModels;

public class AccountSignFlowViewModel : ViewModel
{
    private int _selectedIndex;

    public int SelectedIndex { get => _selectedIndex; set => SetValue(ref _selectedIndex, value); }
}