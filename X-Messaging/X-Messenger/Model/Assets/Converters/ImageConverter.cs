using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace X_Messenger.Model.Assets.Converters;

internal static class ImageConverter
{
    private static readonly object locker = new object();
    public static BitmapImage ToImageSource(byte[] buffer)
    {
        if (buffer is null) return null;

        BitmapImage bitmapImage = null;

        Application.Current.Dispatcher.Invoke(() =>
        {
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();

            }
        });
        return bitmapImage;
    }

    public static byte[] ImageToBytes(BitmapImage bitmapImage)
    {
        MemoryStream ms = null;
        Application.Current.Dispatcher.Invoke(() =>
        {
            using (ms = new MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(ms);
            }
        });
        return ms?.ToArray();
    }

    public static BitmapImage OnLoadImage()
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Image|*.jpg;*.jpeg;*.png;";
        BitmapImage source = null;
        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                source = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Absolute));
            }
            catch
            {
                View.Modals.MessageBox.Show("Выберите файл подходящего формата");
            }
        }

        return source;
    }
}
