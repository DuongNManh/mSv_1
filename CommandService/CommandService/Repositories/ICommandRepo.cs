using CommandService.Models.Entities;
using CommandService.Models.Requests;

namespace CommandService.Repositories
{
    public interface ICommandRepo
    {
        // Methods for Commands
        Task<IEnumerable<Command>> GetAllCommands();
        Task<Command> GetCommandById(int id);
        Task<IEnumerable<Command>> GetCommandsForPlatform(int platId);
        Task<Command> GetCommand(int platId, int commandId);
        Task<Command> CreateCommand(int platId, CommandCreateRequest command);
        Task UpdateCommand(int commandId, CommandUpdateRequest command);
        Task DeleteCommand(int commandId);

        // Methods for Platforms
        Task CreatePlatform(Platform plat);
        Task<IEnumerable<Platform>> GetAllPlatforms();
        bool IsExternalPlatformExists(int platId);
    }
}
