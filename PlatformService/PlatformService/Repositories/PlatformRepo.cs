using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlatformService.Contexts;
using PlatformService.Models.Entities;
using PlatformService.Models.Exceptions;
using PlatformService.Models.Requests;

namespace PlatformService.Repositories
{
    public class PlatformRepo(AppDbContext appDbContext, IMapper mapper) : IPlatformRepo
    {

        public async Task<Platform> CreatePlatform(PlatformCreateRequest plat)
        {
            Platform platform = mapper.Map<Platform>(plat);
            await appDbContext.Platforms.AddAsync(platform);
            await appDbContext.SaveChangesAsync();
            return platform;
        }

        public async Task DeletePlatform(int id)
        {
            var platform = await appDbContext.Platforms.FirstOrDefaultAsync(p => p.Id == id);
            if (platform == null)
                throw new NotFoundException("Platform not found");

            appDbContext.Platforms.Remove(platform);
            await appDbContext.SaveChangesAsync();
        }


        public async Task<IEnumerable<Platform>> GetAllPlatforms()
        {
            return await appDbContext.Platforms.ToListAsync();
        }

        public async Task<Platform> GetPlatformById(int id)
        {
            Platform? platform = await appDbContext.Platforms.FirstOrDefaultAsync(p => p.Id == id);
            if (platform == null)
            {
                throw new NotFoundException("Platform not found");
            }
            return platform;
        }

        public async Task<Platform> UpdatePlatform(int id, PlatformUpdateRequest plat)
        {
            var platform = await appDbContext.Platforms.FirstOrDefaultAsync(p => p.Id == id);
            if (platform == null)
                throw new NotFoundException("Platform not found");

            mapper.Map(plat, platform); // in-place update, no reassignment
            await appDbContext.SaveChangesAsync();
            return platform;
        }

    }
}
