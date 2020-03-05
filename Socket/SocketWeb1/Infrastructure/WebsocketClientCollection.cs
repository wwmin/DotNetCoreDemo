using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocketWeb1.Infrastructure
{
    public class WebsocketClientCollection
    {
        private static List<WebsocketClient> _clients = new List<WebsocketClient>();
        public static void Add(WebsocketClient client)
        {
            _clients.Add(client);
        }

        public static void Remove(WebsocketClient client)
        {
            _clients.Remove(client);
        }

        public static WebsocketClient Get(string clientId)
        {
            var client = _clients.FirstOrDefault(x => x.Id == clientId);
            return client;
        }

        public static List<WebsocketClient> GetRoomClients(string roomNo)
        {
            var client = _clients.Where(x => x.RoomNo == roomNo);
            return client.ToList();
        }
    }
}
