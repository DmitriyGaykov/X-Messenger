using System;
using System.Windows;
using System.Windows.Input;
using X_Messenger.Model.Net;
using X_Messenger.View.Windows.AdminPanel;
using X_Messenger.View.Windows.Edit;

namespace X_Messenger.ViewModel.Windows.MainWindow;

internal partial class MainWindowViewModel
{
    public ICommand OnEditCommand { get; private init; }
    public ICommand OnAdminPanelCommand { get; private init; }

    public Visibility ShowAdminPanel { get; private init; }

    public void OnEdit(object param)
    {
        try
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Client.NavigateTo<EditView>();
            }));
        } catch
        {

        }
    }

    public void OnAdminPanel(object param)
    {
        AdminPanelView adminPanel = new();
        adminPanel.Show();
    }
}
