using Lib.Converters;
using Lib.MessageNamespace;
using Lib.MessageNamespace.ViewMessages;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Test;

class Program
{
    static public void Main(string[] args)
    {
        Socket socket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        IPAddress ip = IPAddress.Parse("127.0.0.1");
        int PORT = 8282;
        EndPoint serv = new IPEndPoint(ip, PORT);

        int idMy = int.Parse(Console.ReadLine());
        int idTo = int.Parse(Console.ReadLine());

        string text = Console.ReadLine() ?? "msg";

        Message msg = new TextMessage(text);

        msg.IDUserFrom = idMy;
        msg.IDUserTo = idTo;

        var bytes = new byte [30_000];
        var fbytes = MyJsonConverter.ToBytes(msg);
        int i = 0;

        foreach(var b  in fbytes)
        {
            bytes[i++] = b;
        }

        socket.SendToAsync(bytes, serv);
        int size;

        while(true)
        {
            size = socket.ReceiveFrom(bytes, ref serv);
            text = Encoding.UTF8.GetString(bytes, 0, size);
            Console.WriteLine(text);
        }
    }
}