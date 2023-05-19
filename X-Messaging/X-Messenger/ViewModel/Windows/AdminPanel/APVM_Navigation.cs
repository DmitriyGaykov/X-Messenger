using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using X_Messenger.View.Windows.AdminPanel.Pages;

namespace X_Messenger.ViewModel.Windows.AdminPanel;

internal partial class AdminPanelViewModel
{
    private readonly Frame navigator;

    public ICommand ToUsersCommand { get; private init; }
    public ICommand ToStatisticsCommand { get; private init; }
    public ICommand ToStickersCommand { get; private init; }

    private void ToUsers(object param)
    {
        Navigate(new Users(this));
        GetUsersAsync();
    }
    private void ToStatistics(object param)
    {
        Navigate(new Statistics(this));
        GetStatsAsync();
    }
    private void ToStickers(object param)
    {
        Navigate(new Stickers(this));
        GetStickersAsync();
    }

    private void Navigate(Page page)
    {
        navigator.Navigate(page);
    }
}
