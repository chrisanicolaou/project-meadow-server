using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Options;

namespace MeadowServer;

public class Worker(ILogger<Worker> logger, IOptions<Config> config, IAgonesManager agonesManager) : BackgroundService
{
    private readonly Config _config = config.Value;

    private List<NetworkStream> _streams = [];

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting TCP listener...");
        var listener = new TcpListener(new IPEndPoint(IPAddress.Any, _config.Port));
        listener.Start();
        await agonesManager.MarkAsReadyAsync();
        var clients = new List<TcpClient>();
        try
        {
            while (clients.Count < 2 && !stoppingToken.IsCancellationRequested)
            {
                var client =  await listener.AcceptTcpClientAsync(stoppingToken);
                clients.Add(client);
                logger.LogInformation("Client connected!");
            }

            var clientTasks = clients.Select(c => HandleClientAsync(c, stoppingToken)).ToList();
            await Task.WhenAll(clientTasks);
            // while (!stoppingToken.IsCancellationRequested)
            // {
            //     // using var client = await listener.
            //     // await Task.Delay(-1, stoppingToken);
            // }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error in TCP listener");
            throw;
        }
        finally
        {
            listener.Stop();
            await agonesManager.MarkForShutdownAsync();
        }
    }

    private async Task HandleClientAsync(TcpClient client, CancellationToken stoppingToken)
    {
        await using var stream = client.GetStream();
        _streams.Add(stream);
        await ProcessIncomingMessages(stream, stoppingToken);
        // var message = $"📅 {DateTime.Now} 🕛";
        // var dateTimeBytes = Encoding.UTF8.GetBytes(message);
        // await stream.WriteAsync(dateTimeBytes, stoppingToken);
        //
        // logger.LogInformation("Sent message: \"{Message}\"", message);
    }

    private async Task ProcessIncomingMessages(NetworkStream stream, CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Memory<byte> buffer = new byte[1024];

            var size = await stream.ReadAsync(buffer, stoppingToken);
            if (size == 0) continue;
        
            var message = Encoding.UTF8.GetString(buffer.Span[..(size)]);
            logger.LogInformation("Received message: \"{Message}\"", message);
            switch (message)
            {
                case "KILL":
                    KillProcess();
                    _ = SendMessage(stream, $"Received KILL request! Shutting down...");
                    break;
                default:
                    await SendMessageToAllClients(message);
                    break;
            }
        }
    }

    private async Task SendMessageToAllClients(string message)
    {
        foreach (var stream in _streams)
        {
            await SendMessage(stream, message);
        }
    }

    private async Task KillProcess()
    {
        agonesManager.MarkForShutdownAsync();
    }

    public async Task SendMessage(NetworkStream stream, string message)
    {
        logger.LogInformation("Sending message: \"{Message}\"", message);
        await stream.WriteAsync(Encoding.UTF8.GetBytes(message));
    }
}