using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace X_Messenger.View.Tags.Buttons
{
    /// <summary>
    /// Логика взаимодействия для RadiusButton.xaml
    /// </summary>
    public partial class RadiusButton : Button
    {
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(CornerRadius), typeof(RadiusButton), new(new CornerRadius(0)));

        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register("From", typeof(Color), typeof(RadiusButton));

        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register("To", typeof(Color), typeof(RadiusButton));

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Brush), typeof(RadiusButton), new(new SolidColorBrush(Colors.White)));

        public RadiusButton()
        {
            InitializeComponent();
        }

        public CornerRadius Radius
        {
            get => (CornerRadius)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }
        public Color To
        {
            get => (Color)GetValue(ToProperty);
            set => SetValue(ToProperty, value);
        }
        public Color From
        {
            get => (Color)GetValue(FromProperty);
            set => SetValue(FromProperty, value);
        }
    }
}
