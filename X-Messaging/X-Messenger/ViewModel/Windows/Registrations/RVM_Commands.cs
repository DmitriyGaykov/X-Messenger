using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using X_Messenger.Model.Database.DBObjects;
using X_Messenger.Model.Database;
using X_Messenger.Model.Net;
using X_Messenger.Model.Objects;
using X_Messenger.View.Assets.Commands;
using X_Messenger.View.Windows.Authorization;
using X_Messenger.View.Windows.MainPage;
using X_Messenger.View.Modals;
using NAudio.CoreAudioApi;

namespace X_Messenger.ViewModel.Windows.Registrations;

internal partial class RegistrationViewModel
{
    private readonly IList<string> errors = new List<string>();

    public ICommand OnLinkCommand { get; private init; } = new RelayCommand(OnLink, o => true);
    public ICommand ClickOnSubmitCommand { get; private init; }

    private static void OnLink(object param)
    {
        Client.NavigateTo<Autorization>();
    }

    private async Task ClickOnSubmitAsync(object? param)
    {
        bool isNameCorrect = checker.CheckName(Nickname);
        bool isLoginCorrect = checker.CheckLogin(Login);
        bool isPassCorrect = checker.CheckPassword(Password);
        bool isRepeatPasswordCorrect = RepeatPassword.Equals(Password) && Password is not null;

        errors.Clear();

        if (!isNameCorrect)
        {
            errors.Add(
                """
                Ошибка при вводе имени. Первая буква имени должна быть большой,
                а остальные маленькие. Имя может быть лишь на одном языке(русский/английский
                """);
        }

        if (!isLoginCorrect)
        {
            errors.Add(
                """
                Ошибка при вводе логина. Логин должен состоять от 6 до 30 числовых и буквенных символов
                """
                );
        }

        if (!isPassCorrect)
        {
            errors.Add(
                """
                Ошибка при вводе пароля. Пароль должен состоять 8-64 символов. В пароле могут быть любые символы.
                """
                );
        }

        if (!isRepeatPasswordCorrect)
        {
            errors.Add(
                """
                Ошибка при подтверждении пароля. Поля пароля и повторения пароля не совпадают
                """
                );
        }

        if (errors.Count() > 0)
        {
            View.Modals.MessageBox.Show(errors);
        }
        else
        {
            User user = new()
            {
                Name = Nickname?.Trim(),
                Password = Password?.Trim(),
                Login = Login?.Trim()
            };

            await DoRegisterAsync(user);
        }
    }

    private async Task DoRegisterAsync(User? user)
    {
        bool res = await client.DoRegistrationAsync(user);

        if (res)
        {
            client.AppSettings.Save();

            using DB db = new();
            user = (await db.GetDatasAsync<DBUser>(new(user))).First();

            client.CurrentUser = user;

            Client.NavigateTo<MainPageView>();
        }
        else
        {
            View.Modals.MessageBox.Show(new[] { "Регистрация не прошла" });
        }
    }
}
