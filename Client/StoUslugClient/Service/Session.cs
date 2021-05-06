using System;

namespace StoUslugClient.Service
{
    public static class Session
    {
        public static bool Saved { get; private set; } = false;
        public static string UserName { get; private set; }
        public static string Password { get; private set; }
        public static Server[] Servers { get; private set; }

        public static void SaveSession(string userName, string password, Server[] servers)
        {
            UserName = userName;
            Password = password;
            Servers = servers;
            Saved = true;
        }
    }

    public class Server
    {
        public string Name { get; set; }
        public Uri Url { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
