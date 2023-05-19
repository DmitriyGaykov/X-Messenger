using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace X_Messenger.Model.Messages;

internal class Sticker
{
    public int Id { get; set; }
    public BitmapImage Source { get; set; }
    public bool IsCurrent { get; set; } = false;

    public Sticker()
    {
        Id = 0;
    }

    public Sticker(int id, BitmapImage source)
    {
        Id = id;
        Source = source;
    }

    public Sticker(Sticker sticker)
    {
        Id = sticker.Id;
        Source = sticker.Source;
    }
}
