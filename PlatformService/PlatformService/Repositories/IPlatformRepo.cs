using PlatformService.Models.Entities;
using PlatformService.Models.Requests;

namespace PlatformService.Repositories
{
    public interface IPlatformRepo
    {
        Task<IEnumerable<Platform>> GetAllPlatforms();

        Task<Platform> GetPlatformById(int id);

        Task<Platform> CreatePlatform(PlatformCreateRequest plat);

        Task<Platform> UpdatePlatform(int id, PlatformUpdateRequest plat);

        Task DeletePlatform(int id);
    }
}
