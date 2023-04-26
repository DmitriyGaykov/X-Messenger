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
using X_Messenger.ViewModel.Windows.MainWindow;

namespace X_Messenger.View.Windows.MainPage
{
    /// <summary>
    /// Логика взаимодействия для MainPageView.xaml
    /// </summary>
    public partial class MainPageView : Window
    {
        private readonly MainWindowViewModel mainPage = new();

        public MainPageView()
        {
            InitializeComponent();

            this.DataContext = mainPage;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mainPage.StartVoice(sender, e);
        }
    }
}
