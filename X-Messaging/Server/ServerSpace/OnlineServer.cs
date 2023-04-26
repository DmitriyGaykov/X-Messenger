using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server.ServerSpace;
public partial class Server
{
    private readonly IDictionary<int, EndPoint> onlineUsers = new Dictionary<int, EndPoint>();
    private readonly IDictionary<int, DateTime> clearAfter = new Dictionary<int, DateTime>();

    private const int WAITTIME = 120; // seconds

    private async void AddOnlineUserAsync(int userId, EndPoint user)
    {
        await Task.Run(() =>
        {
            lock(onlineUsers)
            {
                onlineUsers[userId] = user;
                clearAfter[userId] = DateTime.Now;

                logger.MicroInfo($"Пользователь в сети. ID: { userId }");
            }
        });
    }
    public async void StartOnlineCleanerAsync()
    {
        logger.IInfo("Онлайн контроллер запущен!");
        await Task.Run(() =>
        {
            Thread.CurrentThread.Priority = ThreadPriority.Lowest;

            while (!isExit)
            {
                lock (onlineUsers)
                {
                    var dict = clearAfter.Where(el => (DateTime.Now - el.Value).Seconds > WAITTIME).Select(el => el.Key).ToList();

                    for (int i = 0; i < dict.Count(); i++)
                    {
                        onlineUsers.Remove(dict[i]);
                        clearAfter.Remove(dict[i]);

                        logger.MicroInfo($"Пользователь покинул сеть. ID: {dict[i]}");
                    }
                }
            }
        });
        logger.IInfo("Онлайн контроллер прекратил работу!");
    }
    public bool IsInNetwork(int id) => onlineUsers.ContainsKey(id);
}
