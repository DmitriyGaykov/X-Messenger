using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using X_Messenger.View.Windows.Authorization;
using X_Messenger.View.Windows.Registrations;

namespace X_Messenger.ViewModel.Windows.Authorisation;

internal class AuthorizationViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public RoutedEventHandler ClickOnLinkEvent { get; set; }

    public AuthorizationViewModel()
    {
        ClickOnLinkEvent += ClickOnLink;
    }

    private static void ClickOnLink(object sender, RoutedEventArgs e)
    {
        RegistrationView reg = new();
        reg.Show();

        var window = Window.GetWindow(sender as DependencyObject);
        window?.Close();
    }
}
