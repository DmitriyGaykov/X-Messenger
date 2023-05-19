using Lib.Assets.Checkers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using X_Messenger.Model.Database;
using X_Messenger.Model.Database.DBObjects;
using X_Messenger.Model.Net;
using X_Messenger.Model.Objects;
using X_Messenger.View.Assets.Commands;
using X_Messenger.View.Windows.MainPage;
using X_Messenger.View.Windows.Registrations;

namespace X_Messenger.ViewModel.Windows.Authorisation;

internal partial class AuthorizationViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private readonly FormDataChecker checker = new();
    private readonly Client client = Client.GetClient();

    public ICommand OnSubmitCommand { get; init; }
    public ICommand OnLinkCommand { get; init; }

    public AuthorizationViewModel()
    {
        OnSubmitCommand = new RelayCommand((o) => { OnSubmitAsync(o); }, o => true);
        OnLinkCommand = new RelayCommand(OnLink, o => true);
    }

    public void OnPropertyChanged(string pName)
    {
        if (PropertyChanged is not null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(pName));
        }
    }

    private string login;
    private string password;

    public string Login
    {
        get => login;
        set
        {
            login = value ?? string.Empty;
            OnPropertyChanged("Login");
        }
    }

    public string Password
    {
        get => password;
        set
        {
            password = value ?? string.Empty;
            OnPropertyChanged("Password");
        }
    }

    private readonly IList<string> errors = new List<string>();

    public async Task OnSubmitAsync(object param)
    {
        bool loginCorrect = checker.CheckLogin(Login);
        bool passCorrect = checker.CheckPassword(Password);

        errors.Clear();

        if (!loginCorrect)
        {
            errors.Add("Логин не корректен. Должно быть от 6 до 30 символов");
        }
        if (!passCorrect)
        {
            errors.Add("Пароль не корректен. Должно быть от 8 до 64 символов");
        }
        if (errors.Count > 0)
        {
            View.Modals.MessageBox.Show(string.Join("\n\n", errors));
        }
        else
        {
            User user = new()
            {
                Login = Login.Trim(),
                Password = Password.Trim()
            };

            await Authorize(user);
        }
    }

    public async Task Authorize(User? user)
    {
        var res = await client.DoAuthorizationAsync(user);

        if (res)
        {
            using DB db = new();
            user = (await db.GetDatasAsync<DBUser>(new(user))).First();

            Client.GetClient().CurrentUser = user;
            Client.NavigateTo<MainPageView>();
        }
        else
        {
            View.Modals.MessageBox.Show("Пользователя с такими данными не существует");
        }
    }

    public void OnLink(object param)
    {
        Window reg = new RegistrationView();
        Client.NavigateTo<RegistrationView>();
    }
}
