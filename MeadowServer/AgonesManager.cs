using Agones;
using Microsoft.Extensions.Options;

namespace MeadowServer;

public class AgonesManager : IAgonesManager
{
    private readonly Config _config;
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _appLifetime;
    private readonly AgonesSDK _sdk = new();

    public AgonesManager(ILogger<AgonesManager> logger, IOptions<Config> config, IHostApplicationLifetime appLifetime)
    {
        _config = config.Value;
        _logger = logger;
        _appLifetime = appLifetime;
        RunHealthChecks();
    }

    public async Task MarkAsReadyAsync() => await _sdk.ReadyAsync();

    public async Task MarkForShutdownAsync() => await _sdk.ShutDownAsync();
    
    private async Task HealthCheckAsync() => await _sdk.HealthAsync();

    private async void RunHealthChecks()
    {
        try
        {
            while (!_appLifetime.ApplicationStopping.IsCancellationRequested)
            {
                await HealthCheckAsync();
                await Task.Delay(1000, _appLifetime.ApplicationStopping);
            }
        }
        catch (Exception e)
        {
            throw; // TODO handle exception
        }
    }
}