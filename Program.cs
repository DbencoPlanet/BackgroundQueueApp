using BackgroundQueueApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region Worker Services
builder.Host.ConfigureServices((context, services) =>
{
    services.AddSingleton<MonitorLoop>();
    services.AddHostedService<QueuedHostedService>();
    services.AddSingleton<IBackgroundTaskQueue>(_ =>
    {
        if (!int.TryParse(context.Configuration["QueueCapacity"], out var queueCapacity))
        {
            queueCapacity = 100;
        }
        return new BackgroundTaskQueue(queueCapacity);
    });
});
#endregion

var app = builder.Build();


// Monitor worker config
var monitorLoop = app.Services.GetRequiredService<MonitorLoop>()!;
monitorLoop.StartMonitorLoop();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

