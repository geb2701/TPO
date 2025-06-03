using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text.Json;
using Template.Domain.ExampleWithStringId.Dtos;
using Template.Domain.ExampleWithStringId.Features;
using Template.Resources;

namespace Template.Controllers.v1;

[ApiController]
[Route("api/v{v:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public sealed class ExampleWithStringIdController : ControllerBase
{
    private readonly ILogger<ExampleWithStringIdController> _logger;
    private readonly IMediator _mediator;
    private readonly ActivitySource _activitySource;
    private readonly Counter<long> _commandHitCounter;

    public ExampleWithStringIdController(ILogger<ExampleWithStringIdController> logger, IMediator mediator, Instrumentation instrumentation)
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
    [HttpPost(Name = "AddExampleWithStringId")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ExampleWithStringIdDto>> Add(
        [FromBody] ExampleWithStringIdForCreationDto dto)
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
        var command = new AddExampleWithStringId.Command(dto);
        var commandResponse = await _mediator.Send(command);

        _commandHitCounter.Add(1);

        _logger.LogInformation($"[!!] Entity created with Id {commandResponse.Code}.");

        return CreatedAtRoute("GetExampleWithStringId",
            new { commandResponse.Code },
            commandResponse);
    }

    /// <summary>
    ///     Gets a single record by ID.
    /// </summary>
    [HttpGet("{code}", Name = "GetExampleWithStringId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExampleWithStringIdDto>> Get(string code)
    {
        var query = new GetExampleWithStringId.Query(code);
        var queryResponse = await _mediator.Send(query);
        return Ok(queryResponse);
    }

    /// <summary>
    ///     Gets a list of all records.
    /// </summary>
    [HttpGet(Name = "GetListExampleWithStringId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList([FromQuery] ExampleWithStringIdParametersDto parametersDto)
    {
        var query = new GetExampleWithStringIdList.Query(parametersDto);
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
    [HttpPut("{code}", Name = "UpdateExampleWithStringId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<IActionResult> Update(string code, ExampleWithStringIdForUpdateDto dto)
    {
        var command = new UpdateExampleWithStringId.Command(code, dto);
        await _mediator.Send(command);
        return Ok();
    }

    /// <summary>
    ///     Deletes an existing record.
    /// </summary>
    [HttpDelete("{code}", Name = "DeleteExampleWithStringId")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(string code)
    {
        var command = new DeleteExampleWithStringId.Command(code);
        await _mediator.Send(command);
        return NoContent();
    }
}