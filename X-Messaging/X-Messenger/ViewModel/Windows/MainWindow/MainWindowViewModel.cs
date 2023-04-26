using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace X_Messenger.ViewModel.Windows.MainWindow;

public class MainWindowViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public MouseButtonEventHandler VoiceEvent { get; set; }

    public MainWindowViewModel()
    {
        VoiceEvent = new(StartVoice);
    }

    private bool isStartedVoice = false;

    public void StartVoice(object sender, MouseButtonEventArgs e)
    {
        var img = sender as Image;
        string file = isStartedVoice ? "Pause" : "StartVoiceMessage";

        isStartedVoice = !isStartedVoice;

        img.Source = new BitmapImage(new Uri($"../../../View/Windows/MainPage/{file}.png", UriKind.Relative));
    }
}
