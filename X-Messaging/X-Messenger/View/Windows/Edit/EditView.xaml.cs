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
using X_Messenger.ViewModel.Windows.Edit;

namespace X_Messenger.View.Windows.Edit;

/// <summary>
/// Логика взаимодействия для EditView.xaml
/// </summary>
public partial class EditView : Window
{
    private readonly EditViewModel edit = new();
    public EditView()
    {
        InitializeComponent();

        this.DataContext = edit;
    }
}
