using AutoMapper;
using CommandService.Contexts;
using CommandService.Models.Entities;
using CommandService.Models.Exceptions;
using CommandService.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Repositories
{
    public class CommandRepo(AppDbContext appDbContext, IMapper mapper, ILogger<CommandRepo> logger) : ICommandRepo
    {

        public async Task<Command> CreateCommand(int platId, CommandCreateRequest command)
        {
            try
            {
                var platform = await appDbContext.Platforms
                    .FirstOrDefaultAsync(p => p.ExternalId == platId);

                if (platform == null)
                {
                    throw new NotFoundException($"Platform {platId} not found");
                }

                if (command == null)
                {
                    throw new BadRequestException("Command cannot be null");
                }

                // Map the command request to the command entity
                var commandEntity = mapper.Map<Command>(command);
                commandEntity.PlatformId = platform.Id;  // Use the actual Platform.Id, not the ExternalId

                await appDbContext.Commands.AddAsync(commandEntity);
                await appDbContext.SaveChangesAsync();

                return commandEntity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task CreatePlatform(Platform plat)
        {
            try
            {
                if (plat == null)
                {
                    throw new BadRequestException("Platform cannot be null");
                }
                await appDbContext.Platforms.AddAsync(plat);
                await appDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error creating platform");
                throw;
            }
        }

        public async Task DeleteCommand(int commandId)
        {
            try
            {
                var command = await appDbContext.Commands.FindAsync(commandId);
                if (command == null)
                {
                    throw new NotFoundException($"Command {commandId} not found");
                }
                appDbContext.Commands.Remove(command);
                await appDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error deleting command {CommandId}", commandId);
                throw;
            }
        }

        public async Task<IEnumerable<Command>> GetAllCommands()
        {
            return await appDbContext.Commands.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Platform>> GetAllPlatforms()
        {
            return await appDbContext.Platforms.AsNoTracking().ToListAsync();
        }

        public async Task<Command> GetCommand(int platId, int commandId)
        {
            var platformExists = await appDbContext.Platforms.AnyAsync(p => p.Id == platId);
            if (!platformExists)
            {
                throw new NotFoundException($"Platform {platId} not found");
            }

            var command = await appDbContext.Commands.AsNoTracking()
                .FirstOrDefaultAsync(c => c.PlatformId == platId && c.Id == commandId);

            if (command == null)
            {
                throw new NotFoundException($"Command {commandId} for platform {platId} not found");
            }
            return command;
        }

        public async Task<Command> GetCommandById(int id)
        {
            try
            {
                var command = await appDbContext.Commands.AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id);
                if (command == null)
                {
                    throw new NotFoundException($"Command {id} not found");
                }
                return command;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting command by id {CommandId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Command>> GetCommandsForPlatform(int platId)
        {
            try
            {
                // Check if the platform exists
                var platformExists = await appDbContext.Platforms.AnyAsync(p => p.Id == platId);
                if (!platformExists)
                {
                    throw new NotFoundException($"Platform {platId} not found");
                }
                return await appDbContext.Commands
                    .Where(c => c.PlatformId == platId)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting commands for platform {PlatformId}", platId);
                throw;
            }
        }

        public bool IsExternalPlatformExists(int platId)
        {
            return appDbContext.Platforms.Any(p => p.ExternalId == platId);
        }

        public async Task UpdateCommand(int commandId, CommandUpdateRequest command)
        {
            try
            {
                var commandToUpdate = await appDbContext.Commands.FindAsync(commandId);
                if (commandToUpdate == null)
                {
                    throw new NotFoundException($"Command {commandId} not found");
                }

                // Map the updated properties from the request to the command entity
                mapper.Map(command, commandToUpdate);
                appDbContext.Commands.Update(commandToUpdate);
                await appDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error updating command {CommandId}", commandId);
                throw;
            }
        }
    }
}
