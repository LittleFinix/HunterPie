using HunterPie.Core.Logger;
using HunterPie.GUI.Parts.Sidebar.ViewModels;
using System;

namespace HunterPie.GUI.Parts.Sidebar.Service;

public static class SideBarService
{
    public delegate void SideBarEventHandler(ISideBarElement element);

    public static event SideBarEventHandler? NavigateToElement;
    public static ISideBarElement? CurrentlySelected { get; private set; }

    public static void Navigate(ISideBarElement? element)
    {
        if (element is null)
            return;

        CurrentlySelected = element;

        NavigateToElement?.Invoke(element);

        try
        {
            element.ExecuteOnClick();
        }
        catch (Exception ex) 
        {
            Log.Error($"An error occurred while navigating to {element.Text}");
        }
    }
}
