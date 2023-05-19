using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Input;
using X_Messenger.Model.Assets.Converters;
using X_Messenger.Model.Assets.Messages.StickerMessages;
using X_Messenger.Model.Database;
using X_Messenger.Model.Database.DBObjects;
using X_Messenger.Model.Messages;
using X_Messenger.Model.Net;

namespace X_Messenger.ViewModel.Windows.MainWindow;

internal partial class MainWindowViewModel
{
    public ObservableCollection<Sticker> Stickers { get; set; } = new();
    private readonly IDictionary<int, Sticker> dictSticker = new Dictionary<int, Sticker>();

    public ICommand OnStickerImageCommand { get; init; }

    private async void GetStickersAsync()
    {
        Stickers.Clear();
        dictSticker.Clear();
        using DB db = new();
        var set = await db.GetDataSetFromAsync<DBSticker>();
        DBSticker dbSticker = new();
        Sticker sticker;

        while (set.Read())
        {
            sticker = (Sticker)dbSticker.GetObjectFrom(set);

            await System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Stickers.Add(sticker);
                dictSticker.Add(sticker.Id, sticker);
            });
        }
    }

    private void OnStickerImage(object param)
    {
        Sticker sticker = param as Sticker;
        sticker = dictSticker[sticker.Id];
        SendStickerMessage(sticker);

        ShowStickersMenu = false;
        OnPropertyChanged("ShowStickersMenu");
    }

    private void SendStickerMessage(Sticker sticker)
    {
        StickerMessage msg = new MyStickerMessage(new());

        if(sticker.Id != 0)
            msg.Source = sticker.Source;
        else
        {
            var img = Model.Assets.Converters.ImageConverter.OnLoadImage();

            if(img is null)
            {
                View.Modals.MessageBox.Show("Вы не выбрали фото");
                return;
            }

            msg.Source = img;
        }
        msg.IdFrom = Client.GetClient().CurrentUser.Id.Value;
        msg.IdTo = CurrentUser.Id.Value;

        SendMessageAsync(msg);
        AddToMessageList(msg, false);
    }
}
