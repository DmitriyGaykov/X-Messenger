using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using X_Messenger.ViewModel.Windows.Authorisation;

namespace X_Messenger.View.Windows.Authorization
{
    /// <summary>
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Window
    {
        private readonly AuthorizationViewModel auth = new();

        public Autorization()
        {
            InitializeComponent();

            this.DataContext = auth;

            LinkTo.Click += auth?.ClickOnLinkEvent;
        }
    }
}
