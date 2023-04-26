using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using X_Messenger.View.Windows.Authorization;

namespace X_Messenger.ViewModel.Windows.Registrations;
internal class RegistrationViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public RoutedEventHandler ClickOnLinkEvent { get; set; }

    public RegistrationViewModel()
    {
        ClickOnLinkEvent += ClickOnLink;
    }

    private static void ClickOnLink(object sender, RoutedEventArgs e)
    {
        Autorization auth = new();
        auth.Show();

        var window = Window.GetWindow(sender as DependencyObject);
        window?.Close();
    }
}
