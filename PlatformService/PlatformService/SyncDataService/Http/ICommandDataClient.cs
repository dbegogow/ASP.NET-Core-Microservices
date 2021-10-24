using PlatformService.DTOs;
using System.Threading.Tasks;

namespace PlatformService.SyncDataService.Http
{
    public interface ICommandDataClient
    {
        Task SendPlatformToCommand(PlatformReadDto platform);
    }
}
