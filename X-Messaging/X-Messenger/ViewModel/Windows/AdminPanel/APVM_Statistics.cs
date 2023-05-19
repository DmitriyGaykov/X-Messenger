using System;
using System.Linq;
using System.Windows;
using X_Messenger.Model.Database;
using X_Messenger.Model.Database.DBObjects;

namespace X_Messenger.ViewModel.Windows.AdminPanel;

internal partial class AdminPanelViewModel
{
    private Model.Objects.Statistics stats;

    public Model.Objects.Statistics Stats
    {
        get => stats;
        set
        {
            stats = value;
            OnPropertyChanged("Stats");
        }
    }

    public async void GetStatsAsync()
    {
        using DB db = new DB();
        var stats = await db.GetDatasAsync(new DBStatistics());

        await Application.Current.Dispatcher.InvokeAsync(new Action(() =>
        {
            Stats = stats.First();
        }));
    }
}
