using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWeb1.Hubs
{
    public interface IPostHub
    {
        Task Show(string msg);
        Task SendToAll(string msg);
        Task SendToOthers(string msg);
        Task SendToSelf(string msg);
        Task SendToUser(string msg, string connectionId);
        Task SendToGroup(string msg, string groupName);
        Task AddToGroup(string groupName);
        Task RemoveFromGroup(string groupName);
    }
    public class PostHub : Hub<IPostHub>
    {
        private readonly ILogger<PostHub> _log;
        public PostHub(ILogger<PostHub> log)
        {
            _log = log;
        }

        public async Task SendToAll(string msg)
        {
            await Clients.All.Show(msg);
        }

        public async Task SendToOthers(string msg)
        {
            await Clients.Others.Show(msg);
        }

        public async Task SendToSelf(string msg)
        {
            var id = Context.ConnectionId;
            await Clients.Client(id).Show(msg);
        }

        public async Task SendToGroup(string msg, string groupName)
        {
            await Clients.Group(groupName).Show(msg);
        }

        public async Task SendToUser(string msg, string connectionId)
        {
            await Clients.Client(connectionId).Show(msg);
        }

        public async Task AddToGroup(string groupName)
        {
            var id = Context.ConnectionId;
            await Groups.AddToGroupAsync(id, groupName);
            await Clients.Group(groupName).Show($"{id}:joined the group {groupName}");
        }


        public async Task RemoveFromGroup(string groupName)
        {
            var id = Context.ConnectionId;
            await Groups.RemoveFromGroupAsync(id, groupName);
            await Clients.Group(groupName).Show($"{id}:left the group {groupName}");
        }
        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            _log.LogInformation("hub connectionId:" + connectionId);
            await Clients.Client(connectionId).Show($"用户{connectionId}已登录");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            await Groups.RemoveFromGroupAsync(connectionId, "SignalR Users");
            await Clients.AllExcept(Context.ConnectionId).Show($"{connectionId}已离线");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
