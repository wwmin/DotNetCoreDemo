using AlterNats;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddNats(1, option =>
{
    option = NatsOptions.Default with
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
    return option;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
