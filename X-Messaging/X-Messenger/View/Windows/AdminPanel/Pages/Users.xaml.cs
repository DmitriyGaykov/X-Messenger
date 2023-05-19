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
using X_Messenger.View.Tags.Buttons;

namespace X_Messenger.View.Windows.AdminPanel.Pages;

/// <summary>
/// Логика взаимодействия для Users.xaml
/// </summary>
public partial class Users : Page
{
    private readonly INotifyPropertyChanged viewModel; 

    public Users(INotifyPropertyChanged viewModel)
    {
        InitializeComponent();

        this.viewModel = viewModel;
        this.DataContext = viewModel;
    }
}
