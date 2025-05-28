using Agones;
using Grpc.Core;
using Microsoft.Extensions.Options;

namespace MeadowServer;

public class AgonesManager : IAgonesManager
{
    private readonly Config _config;
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _appLifetime;
    private readonly AgonesSDK _sdk;

    public AgonesManager(ILogger<AgonesManager> logger, IOptions<Config> config, IHostApplicationLifetime appLifetime)
    {
        _sdk = new AgonesSDK(logger: logger);
        _config = config.Value;
        _logger = logger;
        _appLifetime = appLifetime;
        RunHealthChecks();
    }

    public async Task MarkAsReadyAsync()
    {
        _logger.LogInformation("Marking server as ready with Agones SDK...");
        var status = await _sdk.ReadyAsync();
        if (status.StatusCode != StatusCode.OK)
        {
            _logger.LogError("Failed to connect to SDK. Aborting");
        }
    }

    public async Task MarkForShutdownAsync()
    {
        _logger.LogInformation("Shutdown request received, sending to Agones SDK...");
        await _sdk.ShutDownAsync();
    }

    private async Task HealthCheckAsync()
    {
        _logger.LogInformation("Sending health check...");
        try
        {
            await _sdk.HealthAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Health check to Agones failed :/");
        }
    }

    private async void RunHealthChecks()
    {
        try
        {
            while (!_appLifetime.ApplicationStopping.IsCancellationRequested)
            {
                await HealthCheckAsync();
                await Task.Delay(2000, _appLifetime.ApplicationStopping);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Health checks fatal error. Aborting");
        }
    }
}