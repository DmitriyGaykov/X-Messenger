using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using X_Messenger.Model.Assets.Messages;
using X_Messenger.Model.Assets.Messages.VoiceMessages;
using X_Messenger.Model.Net;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using X_Messenger.View.Assets.Commands;
using System.Windows.Documents;
using System.Text;

namespace X_Messenger.ViewModel.Windows.MainWindow;

internal partial class MainWindowViewModel
{
    public bool VoiceStarted { get; set; } = false;

    public void OnVoiceMessage(object param)
    {
        VoiceMessageActive = !VoiceMessageActive;

        if (VoiceMessageActive)
        {
            StartRecord();
        }
        else
        {
            EndRecord();

            var voice = recorder.Buffer;

            Message msg = new();
            msg.IdFrom = Client.GetClient().CurrentUser?.Id.Value ?? 0;
            msg.IdTo = CurrentUser?.Id.Value ?? 0;
            msg.Buffer = voice.ToArray();
            msg.MessageType = Message.TypeOfMessage.Voice;

            SendMessageAsync(msg);

            MyVoiceMessage vmsg = new(msg);
            AddToMessageList(vmsg, false);
        }
    }

    private bool StartRecord()
    {
        try
        {
            recorder.StartRecord();
            VoiceStarted = true;
            OnPropertyChanged("VoiceStarted");
            return true;
        } 
        catch
        {
            return false;
        }
    }

    private bool EndRecord()
    {
        try
        {
            recorder.EndRecord();
            VoiceStarted = false;
            OnPropertyChanged("VoiceStarted");
            return true;
        }
        catch
        {
            return false;
        }
    }

    private void PresentVoiceMessage(Guid id)
    {
        VoiceMessage? msg = MessagesList.Where(el => el.IdMessage == id).First() as VoiceMessage;

        if (msg is null) return;

        StopVoiceMessage();
        recorder.StartVoicePresent(msg.Voice);
    }

    private void StopVoiceMessage()
    {
        try
        {
            recorder.StopVoicePresent();
            isIStoped = true;
        }
        catch
        {
            return;
        }
    }

    private Image image = null;
    private bool isStartedVoice = false;
    private bool needChange = true;
    private bool isIStoped = false;

    public void StartVoice(object sender, MouseButtonEventArgs e)
    {
        if (sender is not null)
        {
            var img = sender as Image;

            if (image is not null && !image.Equals(img))
            {
                StopVoiceMessage();

                needChange = false;
                isStartedVoice = false;
                image.Source = new BitmapImage(new Uri($"../../../View/Windows/MainPage/StartVoiceMessage.png", UriKind.Relative));
            }

            image = img;

            isStartedVoice = !isStartedVoice;

            string file = isStartedVoice ? "Pause" : "StartVoiceMessage";
            image.Source = new BitmapImage(new Uri($"../../../View/Windows/MainPage/{file}.png", UriKind.Relative));

            if (isStartedVoice)
            {
                PresentVoiceMessage(new(image.Uid));
            }
            else
            {
                StopVoiceMessage();
            }
        }
    }
    private void OnStopped(object s, EventArgs e)
    {
        if (needChange)
        {
            isStartedVoice = false;
            image.Source = new BitmapImage(new Uri($"../../../View/Windows/MainPage/StartVoiceMessage.png", UriKind.Relative));
            image = null;
        }
        else
        {
            needChange = true;
        }
    }
}
// 1 