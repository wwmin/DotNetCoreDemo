using AlterNats;

using ConsoleApp1;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;

using System.Net.Http.Json;

var options = NatsOptions.Default with
{
    Url = "nats://127.0.0.1:4222",
    LoggerFactory = new MinimumConsoleLoggerFactory(LogLevel.Information),
    //Serializer = new MessagePackNatsSerializer(),
    ConnectOptions = ConnectOptions.Default with
    {
        Echo = true,
        Username = "nats",
        Password = "P@ssw0rd",
    }
};
// NATS server requires `-m 8222` option
await using var conn = new NatsConnection(options);
conn.OnConnectingAsync = async x => // (host,port)
{
    var health = await new HttpClient().GetFromJsonAsync<NatsHealth>($"http://{x.Host}:8222/healthz");
    if (health == null || health.status != "ok") throw new Exception();

    // if returning another (host, port), TCP connection will use it.
    return x;
};
// for subscriber. await register to NATS server(not means await complete)
var subscription = await conn.SubscribeAsync<Person>("foo", x =>
{
    Console.WriteLine($"Received {x}");
});

// for publisher.
//await conn.PublishAsync("foo", new Person(30, "bar"));

// unsubscribe
//subscription.Dipose();
Console.ReadKey();

public record NatsHealth(string status);
