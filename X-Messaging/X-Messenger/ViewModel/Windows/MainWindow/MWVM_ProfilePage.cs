using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using X_Messenger.View.Windows.MainPage.Pages;

namespace X_Messenger.ViewModel.Windows.MainWindow;

internal partial class MainWindowViewModel
{
    public ICommand ShowProfileCommand { get; init; }
    public ICommand OnCrossProfileCommand { get; init; }

    private bool showProfile = false;

    public bool IsShowedProfile
    {
        get => showProfile;
        set
        {
            showProfile = value;
            OnPropertyChanged("IsShowedProfile");
        }
    }

    public void ShowProfile(object param)
    {
        IsShowedProfile = !IsShowedProfile;
    }
}
