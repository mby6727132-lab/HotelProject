using Hotel.Core.DTOs.Rooms;
using Hotel.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet("/api/hotels/{hotelId:int}/rooms/available")]
    public async Task<ActionResult<IEnumerable<RoomResponseDto>>> GetAvailable(
        [FromRoute] int hotelId, [FromQuery] AvailableRoomsQueryDto query, CancellationToken cancellationToken)
    {
        var rooms = await _roomService.GetAvailableAsync(hotelId, query, cancellationToken);
        return Ok(rooms);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RoomResponseDto>> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var room = await _roomService.GetByIdAsync(id, cancellationToken);
        return room is null ? NotFound() : Ok(room);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RoomResponseDto>> Create([FromBody] CreateRoomDto dto, CancellationToken cancellationToken)
    {
        var created = await _roomService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RoomResponseDto>> Update([FromRoute] int id, [FromBody] UpdateRoomDto dto, CancellationToken cancellationToken)
    {
        var updated = await _roomService.UpdateAsync(id, dto, cancellationToken);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var deleted = await _roomService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
