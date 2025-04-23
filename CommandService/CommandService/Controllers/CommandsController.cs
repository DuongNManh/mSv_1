using AutoMapper;
using CommandService.Models.MetaDatas;
using CommandService.Models.Requests;
using CommandService.Models.Responses;
using CommandService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("api/c/platforms/{platId}/commands")]
    [ApiController]
    public class CommandsController(ICommandRepo commandRepo, IMapper mapper) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetCommandsForPlatform([FromRoute] int platId)
        {
            Console.WriteLine($"--> Getting Commands for Platform {platId} from Command Service");
            var commands = await commandRepo.GetCommandsForPlatform(platId);
            return Ok(ApiResponseBuilder.BuildResponse(
                statusCode: StatusCodes.Status200OK,
                message: "Commands retrieved successfully",
                data: mapper.Map<IEnumerable<CommandResponse>>(commands)
            ));
        }

        [HttpGet]
        [Route("{commandId}")]
        public async Task<IActionResult> GetCommandForPlatform(
            [FromRoute] int platId,
            [FromRoute] int commandId)
        {
            Console.WriteLine($"--> Getting Command {commandId} for Platform {platId} from Command Service");
            var command = await commandRepo.GetCommand(platId, commandId);
            return Ok(ApiResponseBuilder.BuildResponse(
                statusCode: StatusCodes.Status200OK,
                message: "Get Command for Platform successfully",
                data: mapper.Map<CommandResponse>(command)
            ));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCommandForPlatform(
            [FromRoute] int platId,
            [FromBody] CommandCreateRequest commandCreateRequest)
        {
            Console.WriteLine($"--> Creating Command for Platform {platId} from Command Service");
            var command = await commandRepo.CreateCommand(platId, commandCreateRequest);

            // Create route values that match GetCommandForPlatform parameters
            var routeValues = new
            {
                platId = platId,
                commandId = command.Id
            };

            return CreatedAtAction(
                nameof(GetCommandForPlatform),
                routeValues,
                ApiResponseBuilder.BuildResponse(
                    statusCode: StatusCodes.Status201Created,
                    message: "Command created successfully",
                    data: mapper.Map<CommandResponse>(command)
                ));
        }
    }
}
