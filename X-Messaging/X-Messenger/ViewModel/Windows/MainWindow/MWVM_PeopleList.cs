using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using X_Messenger.Model.Assets;
using X_Messenger.Model.Database;
using X_Messenger.Model.Database.DBObjects;
using X_Messenger.Model.Messages;
using X_Messenger.Model.Net;

namespace X_Messenger.ViewModel.Windows.MainWindow;

internal partial class MainWindowViewModel : INotifyPropertyChanged
{
    private string searchText;

    public ObservableCollection<LeftUser> PeopleList { get; private set; } = new();

    public string SearchText
    {
        get => searchText;
        set
        {
            searchText = value;

            OnSearchTextChangedAsync();
        }
    }

    public async Task GetPeopleListAsync()
    {
        using DB db = new();
        var dbUser = new DBLeftUser(Client.GetClient().CurrentUser);
        var list = await db.GetDataSetFromAsync(dbUser);

        int id = 0;
        bool isExpected = false;

        PeopleList.Clear();

        bool isFirst = true;
        LeftUser user;
        while (await list.ReadAsync())
        {
            user = (LeftUser)dbUser.GetObjectFrom(list);

            if(user.Id == Client.GetClient().CurrentUser.Id)
            {
                user.Name = "Вы";
            }

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                PeopleList.Add(user);
            });

            if (isFirst && CurrentUser is null)
            {
                SetCurrentUserAsync(user.Id.Value);
            }
            else if (CurrentUser is not null && isFirst)
            {
                id = CurrentUser.Id.Value;
                isExpected = true;
            }

            if (isExpected && user.Id.Value == id)
            {
                SetCurrentUserAsync(id);
            }

            if (isFirst)
                isFirst = false;
        }
    }

    public async void OnSearchTextChangedAsync()
    {
        await Task.Run(OnSearchTextChanged);
    }

    public void OnSearchTextChanged()
    {
        var list = PeopleList.OrderBy(el => el.Name.LevenshteinDistance(SearchText) + (el.Name.Contains(SearchText) ? -100 : 0));
        Application.Current.Dispatcher.Invoke(() =>
        {
            PeopleList = new(list);
            OnPropertyChanged("PeopleList");
        });
    }
}
