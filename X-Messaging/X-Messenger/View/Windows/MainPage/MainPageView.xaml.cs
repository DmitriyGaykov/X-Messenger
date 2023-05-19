using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using X_Messenger.Model.Database;
using X_Messenger.Model.Database.DBObjects;
using X_Messenger.Model.Messages;
using X_Messenger.View.Windows.Edit;
using X_Messenger.View.Windows.MainPage.Pages;
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
            Page settPage = new SettingsPage(mainPage);
            Page profilePage = new Profile(mainPage);

            ProfilePage.Navigate(profilePage);
            Settings.Navigate(settPage);

            StickerButton.Click += mainPage.OnSticker;
            StickerCross.MouseDown += (sender, e) => mainPage.OnSticker(sender, null);

            Messages.Loaded += (s, e) => mainPage.ObjectMessages = s as ListBox;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mainPage.StartVoice(sender, e);
        }

        private void ClickOnPerson(object sender, RoutedEventArgs e)
        {
            var butt = sender as Button;
            mainPage.ClickOnUser(int.Parse(butt.Uid));
        }
    }
}
