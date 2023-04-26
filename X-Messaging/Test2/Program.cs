using Newtonsoft.Json;
using Lib.MessageNamespace;
using System.Net;
using System.Net.Sockets;
using System.Text;
using NAudio.Wave;
using Lib.MessageNamespace.ViewMessages;
using Lib.MessageNamespace.CallMessages;
using Lib.Converters;

namespace Server;

internal class Program
{
    static void Main(string[] args)
    {
        #region Audio



        //Console.Read();

        #endregion

        #region Socket
        try
        {
            string IP = "192.168.0.108";
            int port = 8282 + 1;
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

           

            int sampleRate = 44100; // частота дискретизации
            int channels = 2; // количество каналов (1 - моно, 2 - стерео)
            int bitDepth = 16; // битность звука


            var waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(sampleRate, bitDepth, channels)
            };
            // Обработчик получения звуковых данных
            waveIn.DataAvailable += (sender, e) =>
            {
                // Здесь можно обрабатывать полученные звуковые данные
                // Например, сохранять их в файл или передавать на удаленный сервер

                var buffer = e.Buffer;
                var msg = new ByteCallMessage(buffer);
                msg.IDUserFrom = id;
                msg.IDUserTo = text.IDUserTo;
                var json = MyJsonConverter.ToJson(msg);
                socket.SendToAsync(Encoding.UTF8.GetBytes(json), endPoint);

            };

            // Начинаем запись звука
            waveIn.StartRecording();

            // Ожидание завершения записи (в данном случае - бесконечное)
            Console.WriteLine("Запись началась. Нажмите Enter, чтобы остановить...");
            Console.ReadLine();

            // Останавливаем запись звука
            waveIn.StopRecording();

            
            Console.WriteLine("........................");

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