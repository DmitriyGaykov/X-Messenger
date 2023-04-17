using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Assets.Logging;
using Server.ServerSpace;

namespace Server;

public class Program
{
    public static void Main(string[] args)
    {
        //Logger.ToConsole = true;

        ServerSpace.Server server = null;

        try
        {
            server = new("192.168.0.108", 8282);

            server.Start();
            server.StartReceiveAsync();
            server.StartOnlineCleanerAsync();
            server.MessageControllerStartAsync();
            server.StartCallServerAsync();
        }
        catch(Exception e)
        {
            Logger.GetLogger().Warning(e.Message);
        }

        StringBuilder command = new();

        do
        {
            command.Clear();
            command.Append(Console.ReadLine());
        } while (command.ToString() is not "exit");

        server?.Close();
        Logger.GetLogger().End();
    }
}
