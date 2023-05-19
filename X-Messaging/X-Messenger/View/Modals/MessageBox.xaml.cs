using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace X_Messenger.View.Modals;

/// <summary>
/// Логика взаимодействия для MessageBox.xaml
/// </summary>
public partial class MessageBox : Window
{
    public ObservableCollection<string> Errors { get; }

    public MessageBox(IEnumerable<string> errors)
    {
        InitializeComponent();
        Errors = new ObservableCollection<string>(errors);
        ErrorListBox.ItemsSource = Errors;
        this.Topmost = true;
    }

    public static void Show(IEnumerable<string> errors)
    {
        var mb = new MessageBox(errors);
        mb.Show();
    }

    public static void Show(string msg)
    {
        var mb = new MessageBox(new[] { msg });
        mb.Show();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        this.Close();

    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}
