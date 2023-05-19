using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using X_Messenger.Model.Assets.Converters;

namespace X_Messenger.Model.Assets.Messages.StickerMessages;

internal abstract class StickerMessage : Message
{
    private BitmapImage source;

    public BitmapImage Source
    { 
        get => source;
        set
        {
            source = value;

            if(value is not null)
                this.Buffer = ImageConverter.ImageToBytes(source);
        }
    }

    public StickerMessage(Message msg) : base(msg)
    {
        this.MessageType = TypeOfMessage.Sticker;

        if(Buffer is not null)
            this.Source = ImageConverter.ToImageSource(Buffer);
    }
}
