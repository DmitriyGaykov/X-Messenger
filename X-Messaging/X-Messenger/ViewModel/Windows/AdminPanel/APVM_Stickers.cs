using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using X_Messenger.Model.Assets.Converters;
using X_Messenger.Model.Assets.Messages.TextMessages;
using X_Messenger.Model.Database;
using X_Messenger.Model.Database.DBObjects;
using X_Messenger.Model.Messages;

namespace X_Messenger.ViewModel.Windows.AdminPanel;

internal partial class AdminPanelViewModel
{
    private ObservableCollection<Sticker> stickers = new();
    private TextMessage editingMsg = null;
    private DateTime last = DateTime.Now;

    public ICommand OnStickerCommand { get; private init; }
    public ICommand OnRemoveStickerCommand { get; private init; }

    public ObservableCollection<Sticker> Stickers
    {
        get => stickers;
        set
        {
            stickers = value;
            OnPropertyChanged("Stickers");
        }
    }

    private async void GetStickersAsync()
    {
        Stickers.Clear();

        using DB db = new();
        var set = await db.GetDataSetFromAsync<DBSticker>();
        DBSticker dbSticker = new();
        Sticker sticker;

        while (await set.ReadAsync())
        {
            sticker = (Sticker)dbSticker.GetObjectFrom(set);

            await System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Stickers.Add(sticker);
            });
        }
    }
    private void OnSticker(object param)
    {
        Sticker sticker = param as Sticker;

        if (sticker is null) return;

        if(sticker.Id == 0)
        {
            AddSticker();
        }
        else
        {
            var dur = DateTime.Now - last;

            last = DateTime.Now;
            if (dur.TotalMilliseconds > 500)
            {
                return;
            }

            sticker.IsCurrent = !sticker.IsCurrent;
            Stickers = new(Stickers.Select(el =>
            {
                if(sticker.Id != el.Id)
                {
                    el.IsCurrent = false;
                }

                return el;
            }));
        }
    }

    private async void AddSticker()
    {
        Sticker sticker = new();
        sticker.Source = ImageConverter.OnLoadImage();

        if(sticker.Source is null)
        {
            return;
        }

        DBSticker dbSticker = new(sticker);
        sticker.Id = await AddStickerToDBAsync(dbSticker);

        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            Stickers.Add(sticker);
        });

        View.Modals.MessageBox.Show($"Стикер добавлен!(ID: {sticker.Id})");
    }
    private async Task<int> AddStickerToDBAsync(DBSticker sticker)
    {
        using DB db = new();
        return await Task.Run(() => db.InsertObject(sticker));
    }
    private async void OnRemoveStickerAsync(object param)
    {
        Sticker? sticker = param as Sticker;

        if (sticker is null) return;

        DBSticker dbSticker = new(sticker);

        using DB db = new();
        await db.DeleteObjectAsync(dbSticker);
        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            Stickers.Remove(sticker);
        });

        View.Modals.MessageBox.Show($"Стикер под ID {sticker.Id} удален!");
    }
}
