﻿using Avalonia.Controls;
using Avalonia.Interactivity;
using HunterPie.Features.Account;
using HunterPie.Features.Account.UseCase;
using System.Diagnostics;

namespace HunterPie.GUI.Parts.Account.Views.Promotional;
/// <summary>
/// Interaction logic for AccountPromotionalView.xaml
/// </summary>
public partial class AccountPromotionalView : UserControl
{
    private const string ACCOUNT_LINK = "https://docs.hunterpie.com/posts/account/";

    public AccountPromotionalView()
    {
        InitializeComponent();
    }

    private void OnCreateAccountClick(object sender, RoutedEventArgs e)
    {
        AccountNavigationService.NavigateToSignUp();
        Close();
    }

    private void OnReadMoreClick(object sender, RoutedEventArgs e) => Process.Start("explorer", ACCOUNT_LINK);

    private void OnCloseClick(object sender, RoutedEventArgs e) => Close();

    public void Close()
    {
        IsVisible = false;
        AccountPromotionalUseCase.MarkAsSeen();
    }
}
