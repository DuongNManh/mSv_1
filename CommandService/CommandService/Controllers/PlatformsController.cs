using AutoMapper;
using CommandService.Models.MetaDatas;
using CommandService.Models.Responses;
using CommandService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("api/c/platforms")]
    [ApiController]
    public class PlatformsController(ICommandRepo commandRepo, IMapper mapper) : ControllerBase
    {

        [HttpPost]
        public IActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST # Command Service");
            return Ok("Inbound test from Platforms Controller");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlatformById(int id)
        {
            // This is a placeholder for the actual implementation
            await Task.Delay(100); // Simulate async work
            return Ok(new { Message = $"Platform with ID {id}" });
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PlatformResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms from Command Service");
            var platforms = await commandRepo.GetAllPlatforms();

            return Ok(ApiResponseBuilder.BuildResponse(
                statusCode: StatusCodes.Status200OK,
                message: "Platforms retrieved successfully",
                data: mapper.Map<IEnumerable<PlatformResponse>>(platforms))
                );
        }
    }
}
