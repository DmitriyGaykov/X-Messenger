using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using X_Messenger.Model.Assets;
using X_Messenger.Model.Database;
using X_Messenger.Model.Database.DBObjects;
using X_Messenger.Model.Objects;

namespace X_Messenger.ViewModel.Windows.AdminPanel;

internal partial class AdminPanelViewModel
{
    public ICommand OnSearchCommand { get; private init; }
    public ICommand OnRemoveAccountCommand { get; private init; }

    private ObservableCollection<User> users;
    private string searchText = "Введите имя...";

    public ObservableCollection<User> Users
    {
        get => users;
        set
        {
            users = value;
            OnPropertyChanged("Users");
        }
    }
    public string SearchText
    {
        get => searchText;
        set
        {
            searchText = value;
            OnPropertyChanged("SearchText");
        }
    }


    public async void GetUsersAsync()
    {
        await Task.Run(() =>
        {
            using DB db = new();
            DBUser dbUser = new();
            User user;

            var set = db.GetDataSetByQuery("select * from getUsers()");

            Application.Current.Dispatcher.Invoke(() =>
            {
                Users.Clear();
            });

            while (set.Read())
            {
                user = (User)dbUser.GetObjectFrom(set);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Users.Add(user);
                });
            }
        });
    }
    public async void OnSearchAsync(object param) => await Task.Run(() => OnSearch(param));
    public void OnSearch(object param)
    {
        var sortedList = Users.OrderBy(el => el.Name.LevenshteinDistance(SearchText) + (el.Name.Contains(SearchText) ? - 200 : 0));

        Application.Current.Dispatcher.Invoke(() =>
        {
            Users = new(sortedList);
        });
    }
    public async void OnRemoveAccountAsync(object param)
    {
        User? user = param as User;

        if(user.Id == CurrentAdmin.Id)
        {
            View.Modals.MessageBox.Show("Вы не можете удалить свой аккаунт!");
            return;
        }

        using DB db = new();
        await db.DeleteObjectAsync(new DBUser(user));

        await Application.Current.Dispatcher.BeginInvoke(() =>
        {
            Users.Remove(user);
        });

        View.Modals.MessageBox.Show($"Аккаунт с ID {user.Id} удален!");
    }
}
