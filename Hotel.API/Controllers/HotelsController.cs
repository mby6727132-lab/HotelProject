using Hotel.API.Extensions;
using Hotel.Core.DTOs.Hotels;
using Hotel.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Controllers;

[ApiController]
[Route("api/hotels")]
public class HotelsController : ControllerBase
{
    private readonly IHotelService _hotelService;

    public HotelsController(IHotelService hotelService)
    {
        _hotelService = hotelService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<HotelResponseDto>>> GetByCity(
        [FromQuery] string city, [FromQuery] string? name, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(city))
            return BadRequest(new { message = "city is required." });

        var hotels = await _hotelService.GetByCityAsync(city, name, cancellationToken);
        return Ok(hotels);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<HotelResponseDto>> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var hotel = await _hotelService.GetByIdAsync(id, cancellationToken);
        return hotel is null ? NotFound() : Ok(hotel);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<HotelResponseDto>> Create([FromBody] CreateHotelDto dto, CancellationToken cancellationToken)
    {
        var created = await _hotelService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<HotelResponseDto>> Update([FromRoute] int id, [FromBody] UpdateHotelDto dto, CancellationToken cancellationToken)
    {
        var updated = await _hotelService.UpdateAsync(id, dto, cancellationToken);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var deleted = await _hotelService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    [HttpPost("{id:int}/favorite")]
    [Authorize]
    public async Task<IActionResult> AddFavorite([FromRoute] int id, CancellationToken cancellationToken)
    {
        var added = await _hotelService.AddFavoriteAsync(User.GetUserId(), id, cancellationToken);
        return added ? NoContent() : NotFound();
    }
}
