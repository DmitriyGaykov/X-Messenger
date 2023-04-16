using Newtonsoft.Json;
using Lib.MessageNamespace;
using System.Net;
using System.Net.Sockets;
using System.Text;
using NAudio.Wave;
using Lib.MessageNamespace.ViewMessages;

namespace Server;

internal class Program
{
    private static bool start = false;
    static void Print(int i, Socket socket, EndPoint endPoint)
    {
        Message text = new TextMessage("Hello world " + i.ToString());
        Console.WriteLine("Enter: ");
        string s = Console.ReadLine();
        Console.WriteLine(s);
        int id = int.Parse(s);
        text.IDUserFrom = id;
        text.IDUserTo = 0;

        byte[] buffer = new byte[4096];

        while (!start) ;
        socket.SendToAsync(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(text)), endPoint);
        
        int size = socket.ReceiveFrom(buffer, ref endPoint);

        string message = Encoding.UTF8.GetString(buffer, 0, size);

        Console.WriteLine(message);

        Console.WriteLine("........................");
    }

    static async void PrintAsync(int i, Socket socket, EndPoint endPoint)
    {
        await Task.Run(() => Print(i, socket, endPoint));
    }
    static void Main(string[] args)
    {
        #region Audio

        //int sampleRate = 54100; // частота дискретизации
        //int channels = 2; // количество каналов (1 - моно, 2 - стерео)
        //int bitDepth = 16; // битность звука


        //var waveIn = new WaveInEvent
        //{
        //    WaveFormat = new WaveFormat(sampleRate, bitDepth, channels)
        //};
        //List<byte> buffer = new();
        //// Обработчик получения звуковых данных
        //waveIn.DataAvailable += (sender, e) =>
        //{
        //    // Здесь можно обрабатывать полученные звуковые данные
        //    // Например, сохранять их в файл или передавать на удаленный сервер

        //    buffer.AddRange(e.Buffer);
        //};

        //// Начинаем запись звука
        //waveIn.StartRecording();

        //// Ожидание завершения записи (в данном случае - бесконечное)
        //Console.WriteLine("Запись началась. Нажмите Enter, чтобы остановить...");
        //Console.ReadLine();

        //// Останавливаем запись звука
        //waveIn.StopRecording();

        //var waveOut = new WaveOutEvent();
        //var bufferStream = new MemoryStream(buffer.ToArray());
        //var audioFileReader = new RawSourceWaveStream(bufferStream, waveIn.WaveFormat);
        //waveOut.Init(audioFileReader);
        //waveOut.Play();

        //Console.Read();

        #endregion

        #region Socket
        try
        {
            string IP = "127.0.0.1";
            int port = 8282;
            var ip = IPAddress.Parse(IP);

            EndPoint endPoint = new IPEndPoint(ip, port);

            Socket socket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            Message text = new TextMessage("Hello world " + 1);
            Console.WriteLine("Enter: ");
            string s = Console.ReadLine();
            int id = int.Parse(s);
            text.IDUserFrom = id;
            text.IDUserTo = int.Parse(Console.ReadLine());

            byte[] buffer = new byte[4096];

            socket.SendTo(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(text)), endPoint);
            while (true)
            {
                int size = socket.ReceiveFrom(buffer, ref endPoint);

                string message = Encoding.UTF8.GetString(buffer, 0, size);

                Console.WriteLine(message);
            }

            Console.WriteLine("........................");
            start = true;

            Console.Read();
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            socket.Dispose();
        } 
        catch(Exception e)
        {
            Console.WriteLine("Ошибка\n" + e);
            Thread.Sleep(20000);
        }
        #endregion
    }
}