using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text.Json;
using Template.Domain.ExampleWithIntId.Dtos;
using Template.Domain.ExampleWithIntId.Features;
using Template.Domain.User.Dtos;
using Template.Resources;

namespace Template.Controllers.v1;

[ApiController]
[Route("api/v{v:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public sealed class ExampleWithIntIdController : ControllerBase
{
    private readonly ILogger<ExampleWithIntIdController> _logger;
    private readonly IMediator _mediator;
    private readonly ActivitySource _activitySource;
    private readonly Counter<long> _commandHitCounter;

    public ExampleWithIntIdController(ILogger<ExampleWithIntIdController> logger, IMediator mediator, Instrumentation instrumentation)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        ArgumentNullException.ThrowIfNull(instrumentation);
        _activitySource = instrumentation.ActivitySource;
        _commandHitCounter = instrumentation.CommandHitCounter;
    }

    /// <summary>
    ///     Add a new record.
    /// </summary>
    [HttpPost(Name = "AddExampleWithIntId")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> Add([FromBody] UserForCreationDto dto)
    {
        using var scope = _logger.BeginScope("{Id}", Guid.NewGuid().ToString("N"));

        // Optional: Manually create an activity. This will become a child of
        // the activity created from the instrumentation library for AspNetCore.
        // Manually created activities are useful when there is a desire to track
        // a specific subset of the request. In this example one could imagine
        // that calculating the forecast is an expensive operation and therefore
        // something to be distinguished from the overall request.
        // Note: Tags can be added to the current activity without the need for
        // a manual activity using Activity.Current?.SetTag()
        using var activity = _activitySource.StartActivity("Add command");
        var command = new AddExampleWithIntId.Command(dto);
        var commandResponse = await _mediator.Send(command);

        _commandHitCounter.Add(1);

        _logger.LogInformation($"[!!] Entity created with Id {commandResponse.Id}.");

        return CreatedAtRoute("GetExampleWithIntId",
            new { commandResponse.Id },
            commandResponse);
    }

    /// <summary>
    ///     Gets a single record by ID.
    /// </summary>
    [HttpGet("{id:int}", Name = "GetExampleWithIntId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> Get(int id)
    {
        var query = new GetExampleWithIntId.Query(id);
        var queryResponse = await _mediator.Send(query);
        return Ok(queryResponse);
    }

    /// <summary>
    ///     Gets a list of all records.
    /// </summary>
    [HttpGet(Name = "GetListExampleWithIntId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList([FromQuery] UserParametersDto parametersDto)
    {
        var query = new GetExampleWithIntIdList.Query(parametersDto);
        var queryResponse = await _mediator.Send(query);

        var paginationMetadata = new
        {
            totalCount = queryResponse.TotalCount,
            pageSize = queryResponse.PageSize,
            currentPageSize = queryResponse.CurrentPageSize,
            currentStartIndex = queryResponse.CurrentStartIndex,
            currentEndIndex = queryResponse.CurrentEndIndex,
            pageNumber = queryResponse.PageNumber,
            totalPages = queryResponse.TotalPages,
            hasPrevious = queryResponse.HasPrevious,
            hasNext = queryResponse.HasNext
        };

        Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(paginationMetadata));

        return Ok(queryResponse);
    }

    /// <summary>
    ///     Updates an entire existing record.
    /// </summary>
    [HttpPut("{id:int}", Name = "UpdateExampleWithIntId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, UserForUpdateDto dto)
    {
        var command = new UpdateExampleWithIntId.Command(id, dto);
        await _mediator.Send(command);
        return Ok();
    }

    /// <summary>
    ///     Deletes an existing record.
    /// </summary>
    [HttpDelete("{id:int}", Name = "DeleteExampleWithIntId")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteExampleWithIntId.Command(id);
        await _mediator.Send(command);
        return NoContent();
    }
}