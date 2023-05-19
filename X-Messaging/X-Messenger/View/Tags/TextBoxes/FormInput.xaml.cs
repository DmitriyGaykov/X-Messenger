using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;

namespace X_Messenger.View.Tags.TextBoxes;

/// <summary>
/// Логика взаимодействия для FormInput.xaml
/// </summary>
public partial class FormInput : TextBox
{
    public static readonly DependencyProperty ImageSourceProperty =
        DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(FormInput));

    public static readonly DependencyProperty InputTextProperty =
        DependencyProperty.Register("InputText", typeof(string), typeof(FormInput));

    public static readonly DependencyProperty InputNameProperty =
        DependencyProperty.Register("InputName", typeof(string), typeof(FormInput));

    public static readonly DependencyProperty SizeProperty =
        DependencyProperty.Register("Size", typeof(double), typeof(FormInput), new(30.0));

    public static readonly DependencyProperty MainColorProperty =
        DependencyProperty.Register("MainColor", typeof(System.Windows.Media.Brush), typeof(FormInput), new(new SolidColorBrush(Colors.White)));

    private TextBox textBox = null;

    public FormInput() : base()
    {
        InitializeComponent();

        Events();
    }

    public string InputName
    {
        get => GetValue(InputNameProperty) as string;
        set => SetValue(InputNameProperty, value);
    }
    public string InputText
    {
        get => GetValue(InputTextProperty) as string;
        set => SetValue(InputTextProperty, (string)value);
    }
    public ImageSource ImageSource
    {
        get => GetValue(ImageSourceProperty) as ImageSource;
        set => SetValue(ImageSourceProperty, value);
    }

    public double Size
    {
        get 
        {
            double res;
            var answer = double.TryParse(GetValue(SizeProperty)?.ToString(), out res);
            return answer ? res : 0.0;
        }
        set => SetValue(SizeProperty, value); 
    }

    public System.Windows.Media.Brush MainColor
    {
        get => GetValue(MainColorProperty) as System.Windows.Media.Brush;
        set => SetValue(MainColorProperty, value);
    }

    private void Events()
    {
        this.TextChanged += TextChangedEvent;
        this.LostFocus += LostFocusEvent;
        this.GotFocus += GotFocusEvent;
    }

    private new void GotFocusEvent(object sender, RoutedEventArgs e)
    {
        var tb = (TextBox)e.OriginalSource;
        if(tb.Text.Trim().Equals(InputText))
        {
            this.Text = tb.Text = string.Empty;
        }
    }

    private new void LostFocusEvent(object sender, RoutedEventArgs e)
    {
        var tb = (TextBox)e.OriginalSource;

        if (tb?.Text == string.Empty)
        {
            this.Text = tb.Text = InputText;
        }
    }

    private new void TextChangedEvent(object sender, TextChangedEventArgs e)
    {
        var tb = (TextBox)e.OriginalSource;
        string changes = tb.Text;
        this.Text = changes;

        if (this.textBox is null)
        {
            this.textBox = tb;
        }
    }
}
