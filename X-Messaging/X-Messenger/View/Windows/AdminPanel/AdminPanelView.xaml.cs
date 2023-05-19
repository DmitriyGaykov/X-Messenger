using System.Windows;
using X_Messenger.Model.Objects;
using X_Messenger.ViewModel.Windows.AdminPanel;

namespace X_Messenger.View.Windows.AdminPanel
{
    /// <summary>
    /// Логика взаимодействия для AdminPanelView.xaml
    /// </summary>
    public partial class AdminPanelView : Window
    {
        private readonly AdminPanelViewModel adminPanel;

        public AdminPanelView()
        {
            InitializeComponent();

            adminPanel = new(Pages);
            this.DataContext = adminPanel;
        }
    }
}
