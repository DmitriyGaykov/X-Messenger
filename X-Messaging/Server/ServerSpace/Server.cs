global using Lib.Assets.Logging;
global using System.Net;
global using System.Net.Sockets;
global using Lib.MessageNamespace;

namespace Server.ServerSpace;

public partial class Server
{
    private readonly string ip;
    private readonly int port;
    private readonly EndPoint endPoint;

    private readonly Logger logger = Logger.GetLogger("server.log");

    private readonly Semaphore semaphore = new(3, 5);

    private bool isExit = false;

    private readonly Socket socket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

    public Server(string ip, int port)
    {
        this.ip = ip;
        this.port = port;
        this.cport = port + 1;

        endPoint = new IPEndPoint(IPAddress.Parse(this.ip), this.port);
        cEndPoint = new IPEndPoint(IPAddress.Parse(this.ip), this.cport);

        logger.IInfo("Создан экземляр класса Server");
    }

    public void Start()
    {
        socket.Bind(endPoint);
        callSocket.Bind(cEndPoint);

        logger.IInfo(
            $"""
            =========================================================
                                                                      
                 Сервер связан с параметрами:                         
                 • Протокол:    UDP                                         
                 • Тип сокета:  Datagram                                  
                                                                      
            =========================================================
                                                                                                                  
                 • IP:          {ip}                                            
                 • PORT:        {port}                                        
                 • InnetFamily: {endPoint.AddressFamily.ToString()}    
                                                                       
            =========================================================
            """);
    }

    public void Close()
    {
        socket?.Close();
        isExit = true;

        logger.IInfo("Сервер закончил работу");
    }
}
