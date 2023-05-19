using Lib.Assets.Checkers;
using Lib.Assets.Objects;
using Lib.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using X_Messenger.Model.Net;
using X_Messenger.View.Assets.Commands;
using X_Messenger.View.Windows.Authorization;
using X_Messenger.View.Windows.MainPage;

namespace X_Messenger.ViewModel.Windows.Registrations;
internal partial class RegistrationViewModel : INotifyPropertyChanged
{
    private readonly Client client = Client.GetClient();

    public event PropertyChangedEventHandler? PropertyChanged;

    private readonly FormDataChecker checker = new();

    private string nickname = "Nickname";
    private string login = "Login";
    private string pass = "Password";
    private string repeatPassword = "Repeat password";

    public string Nickname
    {
        get => nickname;
        set
        {
            nickname = value;
            OnPropertyChanged("Nickname");
        }
    }

    public string Login
    {
        get => login;
        set
        {
            login = value;
            OnPropertyChanged("Login");
        }
    }

    public string Password
    {
        get => pass;
        set
        {
            pass = value;
            OnPropertyChanged("Password");
        }
    }

    public string RepeatPassword
    {
        get => repeatPassword;
        set
        {
            repeatPassword = value;
            OnPropertyChanged("RepeatPassword");
        }
    }


    public RegistrationViewModel()
    {
        ClickOnSubmitCommand = new RelayCommand(param => ClickOnSubmitAsync(param), param => true);
    }

    public void OnPropertyChanged(string pName)
    {
        if (PropertyChanged is not null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(pName));
        }
    }
}
