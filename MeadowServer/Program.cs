using MeadowServer;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<Config>(builder.Configuration);

builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IAgonesManager, AgonesManager>();

var host = builder.Build();
host.Run();