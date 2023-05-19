using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace X_Messenger.View.Windows.AdminPanel.Pages;

/// <summary>
/// Логика взаимодействия для Stickers.xaml
/// </summary>
public partial class Stickers : Page
{
    public Stickers(INotifyPropertyChanged dataContext)
    {
        InitializeComponent();

        this.DataContext = dataContext;
    }
}
