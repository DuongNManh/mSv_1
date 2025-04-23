using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Constants;
using PlatformService.Extensions;
using PlatformService.Models.MetaDatas;
using PlatformService.Models.Requests;
using PlatformService.Models.Responses;
using PlatformService.Repositories;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [ApiController]
    public class PlatformsController(IPlatformRepo repository, IMapper mapper, ICommandDataClient commandDataClient, IMessageBusClient messageBusClient) : ControllerBase
    {

        [HttpGet]
        [Route(ApiEndpointConstant.Platform.PlatformsEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PlatformResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllPlatforms()
        {
            var platforms = await repository.GetAllPlatforms();
            return Ok(ApiResponseBuilder.BuildResponse(
                statusCode: 200,
                message: "Platforms retrieved successfully",
                data: mapper.Map<IEnumerable<PlatformResponse>>(platforms)
            ));
        }

        [HttpGet]
        [Route(ApiEndpointConstant.Platform.PlatformById)]
        [ProducesResponseType(typeof(ApiResponse<PlatformResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPlatformById([FromRoute] int id)
        {
            var platform = await repository.GetPlatformById(id);
            return Ok(ApiResponseBuilder.BuildResponse(
                statusCode: 200,
                message: "Platform retrieved successfully",
                data: mapper.Map<PlatformResponse>(platform)
            ));
        }

        [HttpPost]
        [ValidateModelAttributes]
        [Route(ApiEndpointConstant.Platform.PlatformsEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<PlatformResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePlatform([FromBody] PlatformCreateRequest platform)
        {
            var createdPlatform = mapper.Map<PlatformResponse>(await repository.CreatePlatform(platform));

            // Send sync message
            try
            {
                await commandDataClient.SendPlatformToCommand(createdPlatform);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not send synchronously: {e.Message} - {e.InnerException}");
                throw;
            }

            // Send async message
            try
            {
                var platformPublished = mapper.Map<PlatformPublishRequest>(createdPlatform);
                platformPublished.Event = "Platform_Published";
                messageBusClient.PublishNewPlatform(platformPublished);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not send asynchronously: {e.Message} - {e.InnerException}");
                throw;
            }

            return CreatedAtAction(
                nameof(GetPlatformById),
                new { id = createdPlatform.Id },
                ApiResponseBuilder.BuildResponse(
                statusCode: 201,
                message: "Platform created successfully",
                data: createdPlatform
            ));
        }

        [HttpPut]
        [Route(ApiEndpointConstant.Platform.PlatformById)]
        [ValidateModelAttributes]
        [ProducesResponseType(typeof(ApiResponse<PlatformResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePlatform([FromRoute] int id, [FromBody] PlatformUpdateRequest platform)
        {
            var updatedPlatform = await repository.UpdatePlatform(id, platform);

            return Ok(ApiResponseBuilder.BuildResponse(
                statusCode: 200,
                message: "Platform updated successfully",
                data: mapper.Map<PlatformResponse>(updatedPlatform)
            ));
        }


        [HttpDelete]
        [Route(ApiEndpointConstant.Platform.PlatformById)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePlatform([FromRoute] int id)
        {
            await repository.DeletePlatform(id);
            return Ok(ApiResponseBuilder.BuildResponse(
                statusCode: 200,
                message: "Platform deleted successfully"
            ));
        }

    }
}
