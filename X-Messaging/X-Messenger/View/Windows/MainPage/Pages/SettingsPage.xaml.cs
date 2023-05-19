using System.ComponentModel;
using System.Windows.Controls;
using X_Messenger.View.Modals;

namespace X_Messenger.View.Windows.MainPage.Pages
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public INotifyPropertyChanged ViewModel
        {
            set
            {
                this.DataContext = value;
            }
        }
        public SettingsPage(INotifyPropertyChanged viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }
    }
}
