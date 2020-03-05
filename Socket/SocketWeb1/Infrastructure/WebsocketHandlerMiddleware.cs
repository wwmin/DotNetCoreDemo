using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketWeb1.Infrastructure
{
    public class WebsocketHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _log;
        public WebsocketHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _log = loggerFactory.CreateLogger<WebsocketHandlerMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    string clientId = Guid.NewGuid().ToString();
                    var wsClient = new WebsocketClient
                    {
                        Id = clientId,
                        WebSocket = webSocket
                    };
                    try
                    {
                        await Handle(wsClient);
                    }
                    catch (Exception ex)
                    {
                        _log.LogError(ex, "Echo websocket client {0} err .", clientId);
                        await context.Response.WriteAsync("closed");
                    }
                }
                else
                {
                    context.Response.StatusCode = 404;
                }
            }
            else
            {
                await _next(context);
            }
        }

        private async Task Handle(WebsocketClient webSocket)
        {
            WebsocketClientCollection.Add(webSocket);
            _log.LogInformation($"Websocket client added.");

            WebSocketReceiveResult result = null;
            do
            {
                var buffer = new byte[1024 * 1];
                result = await webSocket.WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text && !result.CloseStatus.HasValue)
                {
                    var msgString = Encoding.UTF8.GetString(buffer);
                    _log.LogInformation($"Websocket client ReceiveAsync message {msgString}.");
                    var message = JsonConvert.DeserializeObject<Message>(msgString);
                    message.SendClientId = webSocket.Id;
                    MessageRoute(message);
                }
            } while (!result.CloseStatus.HasValue);
            WebsocketClientCollection.Remove(webSocket);
            _log.LogInformation($"Websocket client closed");
        }

        private void MessageRoute(Message message)
        {
            var client = WebsocketClientCollection.Get(message.SendClientId);
            switch (message.action)
            {
                case "join":
                    client.RoomNo = message.msg;
                    client.SendMessageAsync($"{message.nick} join room {client.RoomNo} success.");
                    _log.LogInformation($"Websocket client {message.SendClientId} join room {client.RoomNo}.");
                    break;
                case "send_to_room":
                    if (string.IsNullOrWhiteSpace(client.RoomNo))
                    {
                        break;
                    }
                    var clients = WebsocketClientCollection.GetRoomClients(client.RoomNo);
                    clients.ForEach(x =>
                    {
                        x.SendMessageAsync(message.nick + " : " + message.msg);
                    });
                    _log.LogInformation($"Websocket client {message.SendClientId} send message {message.msg} to room {client.RoomNo}");
                    break;
                case "leave":
                    var roomNo = client.RoomNo;
                    client.RoomNo = "";
                    client.SendMessageAsync($"{message.nick} leave {roomNo} success .");
                    _log.LogInformation($"Websocket client {message.SendClientId} leave room {roomNo}");
                    break;
                default:
                    break;
            }
        }
    }
}
