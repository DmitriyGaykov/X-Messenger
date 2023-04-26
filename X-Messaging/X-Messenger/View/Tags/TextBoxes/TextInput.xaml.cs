using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace X_Messenger.View.Tags.TextBoxes;

/// <summary>
/// Логика взаимодействия для TextInput.xaml
/// </summary>
public partial class TextInput : TextBox
{
    public static readonly DependencyProperty PlaceholderProperty =
        DependencyProperty.Register("Placeholder", typeof(string), typeof(TextInput), new(string.Empty, null, CoercePlaceholder));

    public TextInput()
    {
        InitializeComponent();

        this.LostFocus += LostFocusPlaceholderEvent;
        this.GotFocus += GotFocusPlaceholderEnter;
    }


    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set
        {
            SetValue(PlaceholderProperty, value);

            if (string.IsNullOrWhiteSpace(this.Text))
            {
                this.Text = value;
            }
        } 
    }

    private void LostFocusPlaceholderEvent(object sender, RoutedEventArgs e)
    {
        if(this?.Text == string.Empty)
        {
            this.Text = Placeholder;
        }
    }

    private void GotFocusPlaceholderEnter(object sender, RoutedEventArgs e)
    {
        var tb  = e.OriginalSource as TextBox;
        if(tb?.Text == Placeholder)
        {
            this.Text = string.Empty;
        }
    }

    private static object CoercePlaceholder(DependencyObject dpo, object value)
    {
        ((TextInput)dpo).Text = value as string ?? string.Empty;
        return value;
    }
}
