namespace MeadowServer;

public interface IAgonesManager
{
    Task MarkAsReadyAsync();
    Task MarkForShutdownAsync();
}