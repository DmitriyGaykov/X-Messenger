using Lib.Assets.Checkers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using X_Messenger.Model.Assets.Converters;
using X_Messenger.Model.Database;
using X_Messenger.Model.Database.DBObjects;
using X_Messenger.Model.Net;
using X_Messenger.Model.Objects;
using X_Messenger.View.Assets.Commands;
using X_Messenger.View.Windows.MainPage;

namespace X_Messenger.ViewModel.Windows.Edit;

internal class EditViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private readonly FormDataChecker checker = new();

    public ICommand OnSubmitCommand { get; set; }
    public ICommand OnLoadImageCommand { get; set; }
    public ICommand OnCancelCommand { get; set; }

    public User CurrentUser { get; set; }

    private BitmapImage source;
    public BitmapImage Source
    {
        get => source;
        set
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                source = value;
                OnPropertyChanged(nameof(Source));
            });
        }
    }

    private string name;
    public string Name
    {
        get => name;
        set
        {
            name = value;
            OnPropertyChanged("Name");
        }
    }

    private string description;
    public string Description
    {
        get => description;
        set
        {
            description = value;
            OnPropertyChanged("Description");
        }
    }

    private string login = "Логин";
    public string Login
    {
        get => login;
        set
        {
            login = value;
            OnPropertyChanged("Login");
        }
    }

    private string password = "Пароль";
    public string Password
    {
        get => password;
        set
        {
            password = value;
            OnPropertyChanged("Password");
        }
    }

    private string rePass = "Повторите пароль";
    public string RePass
    {
        get => rePass;
        set
        {
            rePass = value;
            OnPropertyChanged("RePass");
        }
    }


    public EditViewModel()
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            OnSubmitCommand = new RelayCommand(OnSubmit, o => true);
            OnLoadImageCommand = new RelayCommand(OnLoadImage, o => true);
            OnCancelCommand = new RelayCommand(OnCancel, o => true);

            CurrentUser = Client.GetClient().CurrentUser;
            Name = CurrentUser.Name;
            Description = CurrentUser.Description;
            Source = CurrentUser.Source;
        });
    }

    public void OnLoadImage(object param)
    {
        Source = ImageConverter.OnLoadImage() ?? Source;
    }

    public void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged is not null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public void OnCancel(object param)
    {
        Client.NavigateTo<MainPageView>();
    }

    public void OnSubmit(object param)
    {
        bool nameCorrect = checker.CheckName(Name) || Name == CurrentUser.Name;
        bool loginCorrect = checker.CheckLogin(Login) || Login == "Логин";
        bool passCorrect = checker.CheckPassword(Password) || Password == "Пароль";
        bool repassCorrect = RePass.Equals(Password) || RePass == "Повторите пароль";

        Description = (string.IsNullOrWhiteSpace(Description) || Description == CurrentUser?.Description) ? CurrentUser?.Description : Description;

        List<string> errors = new();

        if (!nameCorrect)
        {
            errors.Add("Имя не корректно!");
        }
        if (!loginCorrect)
        {
            errors.Add("Логин не корректен!");
        }
        if (!passCorrect)
        {
            errors.Add("Пароль не корректен!");
        }
        if (!repassCorrect)
        {
            errors.Add("Пароли не совпадают!");
        }
        if(Description.Length > 300)
        {
            errors.Add("Описание не может быть больше 300 символов!");
        }

        if (errors.Count > 0)
        {

            View.Modals.MessageBox.Show(string.Join("\n", errors));
        }
        else
        {
            User user = new User();
            user.Id = CurrentUser.Id;
            user.Name = Name == CurrentUser.Name ? null : Name;
            user.Login = Login == "Логин" ? null : Login;
            user.Password = Password == "Пароль" ? null : Password;
            user.Description = Description == CurrentUser.Description ? null : Description;
            user.Source = Source;

            GetAnswerAsync(user);
        }
    }

    public async void GetAnswerAsync(User? user)
    {
        if (user is null) return;

        var client = Client.GetClient();
        int code = await client.EditAsync(user);

        switch (code)
        {
            case 1:
                View.Modals.MessageBox.Show("Успешно!");

                using (DB db = new())
                    client.CurrentUser = (await db.GetDatasAsync<DBUser>(new DBUser(user))).First();

                Client.NavigateTo<MainPageView>();
                break;
            case -1:
                View.Modals.MessageBox.Show("Не успешно. Логин занят");
                break;
            case -2:
                View.Modals.MessageBox.Show("Не успешно. Данные не загрузились");
                break;
        }
    }
}
